using ServeyBasket.Authentication.Filters;
using ServeyBasket.Contracts.Roles;
using ServeyBasket.Services.Roles;

namespace ServeyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet("")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> GetAll() =>
        Ok(await _roleService.GetAllAsync());
    [HttpGet("{id}")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _roleService.GetByIdAsync(id);

        return result.IsSuccess
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
    [HttpPost("")]
    [HasPermission(Permissions.AddRoles)]
    public async Task<IActionResult> Add(RoleRequest request)
    {
        var result = await _roleService.AddAsync(request);

        return result.IsSuccess
            ? Ok(result.Value) 
            : result.ToProblem();
    }
    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] RoleRequest request)
    {
        var result = await _roleService.UpdateAsync(id, request);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpDelete("{id}/toggle-status")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> ToggleStatusAsync([FromRoute] string id)
    {
        var result = await _roleService.ToggleStatusAsync(id);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}
