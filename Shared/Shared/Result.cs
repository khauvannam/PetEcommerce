using Shared.Common;

namespace Shared.Shared;

public class Result
{
    private protected Result(bool isSuccess, ErrorType errorType)
    {
        ErrorType = errorType;
        IsSuccess = isSuccess;
    }

    public ErrorType ErrorType { get; private set; }
    private bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success<T>(T value) => new(value, true, Errors.None);

    public static Result Success() => new(true, Errors.None);

    public static Result<T> Failure<T>(ErrorType errorType) => new(default, false, errorType);

    public static Result Failure(ErrorType errorType) => new(false, errorType);
}

public class Result<T> : Result
{
    internal Result(T? value, bool isSuccess, ErrorType error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    private readonly T? _value;
    public T Value => _value!;
}
