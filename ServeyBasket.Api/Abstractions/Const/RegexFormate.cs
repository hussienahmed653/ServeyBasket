namespace ServeyBasket.Abstractions.Const;

public static class RegexFormate
{
    public static string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
}
