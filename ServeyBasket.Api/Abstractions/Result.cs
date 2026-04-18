namespace ServeyBasket.Abstractions;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new InvalidOperationException();
        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public Error Error { get; } = default!;

    public static Result Success() => new(true, Error.None);
    public static Result Failuer(Error error) => new(false, error);
    public static Result<T> Success<T>(T value) => new(value, true, Error.None);
    public static Result<T> Failuer<T>(Error error) => new(default, false, error);
}

public class Result<T>(T? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    private readonly T? _value = value;
    public T? Value => IsSuccess
        ? _value
        : throw new InvalidOperationException();
}
