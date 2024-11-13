namespace Domain.Shared;

public class Result<T>
{
    protected internal Result(T? data, bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = IsSuccess;
                Error = error;
                Data = data;
                break;
        }
    }

    public T? Data { get;}
    public Error Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success(T data)
    {
        return new Result<T>(data, true, Error.None);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(default, false, error);
    }
}