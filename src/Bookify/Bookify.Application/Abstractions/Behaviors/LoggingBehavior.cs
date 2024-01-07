using Bookify.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;
         
        if (cancellationToken.IsCancellationRequested)
        {
            return default!;
        }

        try
        {
            _logger.LogInformation("Executing command {Command}", name);
            var result = await next();
            _logger.LogInformation("Command {Command} processed successfully", name);

            return result;
        }
        catch (Exception)
        {
            _logger.LogError("Command {Command} processing failed", name);
            throw;
        }
    }
}
