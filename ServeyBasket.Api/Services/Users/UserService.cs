using ServeyBasket.Contracts.Users;
using ServeyBasket.Services.Roles;

namespace ServeyBasket.Services.Users;

public class UserService(UserManager<ApplicationUser> userManager, IRoleService roleService, ServeyBasketDbContext context) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IRoleService _roleService = roleService;
    private readonly ServeyBasketDbContext _context = context;

    public async Task<IEnumerable<UserResponse>> GetAllAsync() =>
        await (from u in _context.Users
                     join ur in _context.UserRoles
                     on u.Id equals ur.UserId
                     join r in _context.Roles
                     on ur.RoleId equals r.Id into roles
                     where (!roles.Any(x => x.Name == DefaultRoles.Member))
                     select new
                     {
                         u.Id,
                         u.FirstName,
                         u.LastName,
                         u.Email,
                         u.IsDisabled,
                         Roles = roles.Select(x => x.Name!).ToList()
                     })
                     .Where(x => !x.IsDisabled)
                     .GroupBy(u => new {u.Id, u.FirstName, u.LastName, u.Email, u.IsDisabled})
                     .Select(u => new UserResponse
                     (
                         u.Key.Id,
                         u.Key.FirstName,
                         u.Key.LastName,
                         u.Key.Email,
                         u.Key.IsDisabled,
                         u.SelectMany(x => x.Roles)
                     ))
                     .ToListAsync();
    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        if(await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failuer<UserResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);

        var response = (user, roles).Adapt<UserResponse>();
        return Result.Success(response);
    }
    public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request)
    {
        var emailExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email);
        if(emailExists)
            return Result.Failuer<UserResponse>(UserErrors.DuplicatedEmail);

        var allowedRoles = await _roleService.GetAllAsync();

        if(request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
            return Result.Failuer<UserResponse>(RoleErrors.InvalidRoles);

        var user = request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, request.Password);
        if(result.Succeeded)
        {
            var userRoles = await _userManager.AddToRolesAsync(user, request.Roles);
            var response = (user, request.Roles).Adapt<UserResponse>();
            return Result.Success(response);
        }

        var error = result.Errors.First();
        return Result.Failuer<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> UpdateAsync(string id, UpdateUserRequest request)
    {
        if(await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failuer(UserErrors.UserNotFound);

        var emailExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id);
        if(emailExists)
            return Result.Failuer(UserErrors.DuplicatedEmail);

        var allowedRoles = await _roleService.GetAllAsync();
        if(request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
            return Result.Failuer(RoleErrors.InvalidRoles);

        user = request.Adapt(user);
        var result = await _userManager.UpdateAsync(user);
        if(result.Succeeded)
        {
            await _context.UserRoles.
                Where(x => x.UserId == id)
                .ExecuteDeleteAsync();

            await _userManager.AddToRolesAsync(user, request.Roles);

            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failuer(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> ToggleStatusAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failuer(UserErrors.UserNotFound);

        user.IsDisabled = !user.IsDisabled;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failuer(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> Unlock(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failuer(UserErrors.UserNotFound);

        var result = await _userManager.SetLockoutEndDateAsync(user, null);

        if(result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failuer(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(x => x.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .SingleAsync();

        return Result.Success(user);
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        await _userManager.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(u => u.FirstName, request.FirstName)
                .SetProperty(u => u.LastName, request.LastName)
            );

        return Result.Success();
    }
    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var changed = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);
        if(changed.Succeeded)
            return Result.Success();

        var error = changed.Errors.First();
        return Result.Failuer(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

}
