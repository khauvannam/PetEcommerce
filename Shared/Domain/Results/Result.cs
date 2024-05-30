namespace Shared.Domain.Results;

public class Result
{
    private protected Result(bool isSuccess, ErrorType errorType)
    {
        ErrorType.Add(errorType);
        IsSuccess = isSuccess;
    }

    private Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public List<ErrorType> ErrorType { get; } = [];
    private bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success<T>(T value) => new(value, true, Errors.None);

    public static Result Success() => new(true, Errors.None);

    public static Result<T> Failure<T>(ErrorType errorType) => new(default, false, errorType);

    public static Result Failure(ErrorType errorType) => new(false, errorType);

    public static Result Create(bool isSuccess)
    {
        return new(isSuccess);
    }

    public void AddResultList(ErrorType errorType)
    {
        ErrorType.Add(errorType);
    }
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
