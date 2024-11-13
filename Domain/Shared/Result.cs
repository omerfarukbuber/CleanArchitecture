namespace Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
        
    }

    public Error Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
}