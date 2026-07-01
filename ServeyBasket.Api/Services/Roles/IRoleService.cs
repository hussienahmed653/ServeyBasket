using ServeyBasket.Contracts.Roles;

namespace ServeyBasket.Services.Roles;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync();
    Task<Result<RoleDetailResponse>> GetByIdAsync(string id);
    Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request);
    Task<Result> UpdateAsync(string id, RoleRequest request);
    Task<Result> ToggleStatusAsync(string id);
}
