using Microsoft.AspNetCore.Http;

namespace Domain.Shared.Results;

public class ApiResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Microsoft.AspNetCore.Http.Results.Problem(
            title: GetTitle(result.Error),
            detail: GetDetail(result.Error),
            type: GetType(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            extensions: GetErrors(result));


        static string GetTitle(Error error) => error.Type switch
        {
            ErrorType.Failure => error.Code,
            ErrorType.Validation => error.Code,
            ErrorType.NotFound => error.Code,
            ErrorType.Conflict => error.Code,
            _ => "Server Failure"
        };
        
        static string GetDetail(Error error) => error.Type switch
        {
            ErrorType.Failure => error.Description,
            ErrorType.Validation => error.Description,
            ErrorType.NotFound => error.Description,
            ErrorType.Conflict => error.Description,
            _ => "Server Failure"
        };
        
        static string GetType(ErrorType errorType) => errorType switch
        {
            ErrorType.Failure => "0",
            ErrorType.Validation => "1",
            ErrorType.NotFound => "2",
            ErrorType.Conflict => "3",
            _ => "Server Failure"
        };
        
        static int GetStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        static Dictionary<string, object?>? GetErrors(Result result)
        {
            return new Dictionary<string, object?>
            {
                {
                    "errors", new[]
                    {
                        result.Error
                    }
                }
            };
        }
    }
}