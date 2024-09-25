namespace BasedDomain.Results;

public class Result
{
    private protected Result(bool isSuccess, ErrorType errorType)
    {
        ErrorTypes.Add(errorType);
        IsSuccess = isSuccess;
    }

    private protected Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public List<ErrorType> ErrorTypes { get; } = [];
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

    public static Result Create(bool isSuccess = false)
    {
        return new Result(isSuccess);
    }

    public static Result<T> Create<T>(bool isSuccess)
    {
        return new Result<T>(isSuccess);
    }

    public void AddResultList(ErrorType errorType)
    {
        ErrorTypes.Add(errorType);
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

    internal Result(bool isSuccess)
        : base(isSuccess) { }

    public T Value => _value!;
}
