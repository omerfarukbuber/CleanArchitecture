using Domain.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behavior;

public class LoggingPipeLineBehavior<TRequest, TResponse>(ILogger<LoggingPipeLineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{

    private readonly ILogger<LoggingPipeLineBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting request {@RequestName}, {@DateTime}", typeof(TRequest).Name, DateTime.UtcNow);
        
        var result = await next();

        if (result.IsFailure)
        {
            _logger.LogInformation("Request failed {@RequestName}, {@Error}, {@DateTime}",
                typeof(TRequest).Name,
                result.Error,
                DateTime.UtcNow);
        }

        _logger.LogInformation("Completed request {@RequestName}, {@DateTime}", typeof(TRequest).Name, DateTime.UtcNow);

        return result;
    }
}