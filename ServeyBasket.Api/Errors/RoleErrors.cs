namespace ServeyBasket.Errors;

public static class RoleErrors
{
    public static readonly Error RoleNotFound =
        new("Role.NotFound", "Role is not found", statusCode: StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedRole =
        new("Role.DuplicatedRole", "Duplicated Role", statusCode: StatusCodes.Status409Conflict);

    public static readonly Error InvalidPermissions =
        new("Role.InvalidPermissions", "Invalid permissions", StatusCodes.Status400BadRequest);


}
