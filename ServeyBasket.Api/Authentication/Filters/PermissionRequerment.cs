namespace ServeyBasket.Authentication.Filters;

public class PermissionRequerment(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
