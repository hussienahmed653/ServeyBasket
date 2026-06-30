namespace ServeyBasket.Authentication.Filters;

public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> authorizationOptions) : DefaultAuthorizationPolicyProvider(authorizationOptions)
{
    public AuthorizationOptions _authorizationOptions { get; } = authorizationOptions.Value;

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
            return policy;

        var permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequerment(policyName))
            .Build();

        _authorizationOptions.AddPolicy(policyName, permissionPolicy);

        return permissionPolicy;
    }
}
