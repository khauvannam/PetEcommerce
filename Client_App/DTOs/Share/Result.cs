using Client_App.DTOs.Products.Responses;

namespace Client_App.DTOs.Share;

public class Result
{
    private protected Result(bool isSuccess, ErrorType errorType)
    {
        ErrorTypes = errorType;
        IsSuccess = isSuccess;
    }

    public ErrorType ErrorTypes { get; }
    private bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, true, Errors.None);
    }

    public static Result Success()
    {
        return new Result(true, Errors.None);
    }

    public static Result<T> Failure<T>(ErrorType errorType)
    {
        return new Result<T>(default, false, errorType);
    }

    public static Result Failure(ErrorType errorType)
    {
        return new Result(false, errorType);
    }
}

public class Result<T> : Result
{
    private readonly T? _value;

    internal Result(T? value, bool isSuccess, ErrorType error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value => _value!;
}
