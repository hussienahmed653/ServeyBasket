using ServeyBasket.Contracts.Roles;

namespace ServeyBasket.Services.Roles;

public class RoleService(RoleManager<ApplicationRole> roleManager, ServeyBasketDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ServeyBasketDbContext _context = context;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync() =>
        await _roleManager.Roles
        .Where(x => !x.IsDefault && !x.IsDeleted)
        .ProjectToType<RoleResponse>()
        .ToListAsync();

    public async Task<Result<RoleDetailResponse>> GetByIdAsync(string id)
    {
        if(await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failuer<RoleDetailResponse>(RoleErrors.RoleNotFound);
        var permisstions = await _roleManager.GetClaimsAsync(role);
        return Result.Success(new RoleDetailResponse(id, role.Name!, role.IsDeleted, permisstions.Select(x => x.Value)));
    }

    public async Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request)
    {
        var roleIsExist = await _roleManager.RoleExistsAsync(request.Name);
        if(roleIsExist)
            return Result.Failuer<RoleDetailResponse>(RoleErrors.DuplicatedRole);

        var allowedPermissions = Permissions.GetAllPermissions();

        if(request.Permissions.Except(allowedPermissions).Any())
            return Result.Failuer<RoleDetailResponse>(RoleErrors.InvalidPermissions);

        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            var permissions = request.Permissions
                .Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = role.Id
                });

            await _context.AddRangeAsync(permissions);
            await _context.SaveChangesAsync();

            var response = new RoleDetailResponse(role.Id, role.Name, role.IsDeleted, request.Permissions);

            return Result.Success(response);
        }

        var error = result.Errors.First();

        return Result.Failuer<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> UpdateAsync(string id, RoleRequest request)
    {
        if(await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failuer(RoleErrors.RoleNotFound);

        var duplicatedRole = await _context.Roles.AnyAsync(x => x.Id != id && x.Name == request.Name);
        if(duplicatedRole)
            return Result.Failuer(RoleErrors.DuplicatedRole);

        var allowedPermissions = Permissions.GetAllPermissions();
        if(request.Permissions.Except(allowedPermissions).Any())
            return Result.Failuer<RoleDetailResponse>(RoleErrors.InvalidPermissions);

        role.Name = request.Name;
        var result = await _roleManager.UpdateAsync(role);

        if(result.Succeeded)
        {
            var currentPermissions = await _context.RoleClaims
                .Where(x => x.RoleId == id)
                .Select(v => v.ClaimValue)
                .ToListAsync();

            var newPermissions = request.Permissions.Except(currentPermissions)
                .Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = role.Id
                });
            var removedPermissions = currentPermissions.Except(request.Permissions);

            await _context.RoleClaims
                .Where(x => x.RoleId == id && removedPermissions.Contains(x.ClaimValue))
                .ExecuteDeleteAsync();

            await _context.AddRangeAsync(newPermissions);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        var error = result.Errors.First();

        return Result.Failuer<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> ToggleStatusAsync(string id)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failuer(RoleErrors.RoleNotFound);

        role.IsDeleted = !role.IsDeleted;

        await _roleManager.UpdateAsync(role);
        return Result.Success();
    }
}
