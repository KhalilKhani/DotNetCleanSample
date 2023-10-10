namespace CleanSample.Utility;

public class Result
{
    private readonly Error? _error;
    public bool IsSuccess { get; }
    public bool IsFailure { get; }

    public Error Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("Operation Succeed, so there is no error.");
            return _error ?? throw new NullReferenceException("Error is null.");
        }
    }

    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("A successful result should not have an error.");
        if (!isSuccess && error == null)
            throw new InvalidOperationException("A failed result should have an error.");

        IsSuccess = isSuccess;
        IsFailure = !isSuccess;
        _error = error;
    }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
}

public class Result<T> : Result
{
    private readonly T? _value;

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Operation failed, so there is no value.");
            return _value ?? throw new InvalidOperationException("Value is null.");
        }
    }

    private Result(T value) : base(true, null)
    {
        _value = value;
    }

    private Result(Error? error) : base(false, error)
    {
        _value = default;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public new static Result<T> Failure(Error error)
    {
        return new Result<T>(error);
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }

    public static implicit operator Result<T>(Error error)
    {
        return Failure(error);
    }
}