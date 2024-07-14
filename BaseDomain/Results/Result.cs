namespace BaseDomain.Results;

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

    public static Result<T> Success<T>(T value) => new(value, true, Errors.None);

    public static Result Success() => new(true, Errors.None);

    public static Result<T> Failure<T>(ErrorType errorType) => new(default, false, errorType);

    public static Result Failure(ErrorType errorType) => new(false, errorType);

    public static Result Create(bool isSuccess = false)
    {
        return new(isSuccess);
    }

    public static Result<T> Create<T>(bool isSuccess)
    {
        return new(isSuccess);
    }

    public void AddResultList(ErrorType errorType)
    {
        ErrorTypes.Add(errorType);
    }
}

public class Result<T> : Result
{
    internal Result(T? value, bool isSuccess, ErrorType error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    internal Result(bool isSuccess)
        : base(isSuccess) { }

    private readonly T? _value;

    public T Value => _value!;
}
