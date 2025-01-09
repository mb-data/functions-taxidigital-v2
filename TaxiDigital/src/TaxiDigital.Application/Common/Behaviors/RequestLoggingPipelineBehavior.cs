using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.Common.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Get the name of the request type for logging
        string requestName = typeof(TRequest).Name;

        // Start a stopwatch to measure execution time
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Use LogContext to add structured properties to all logs within this scope
        using (LogContext.PushProperty("RequestType", requestName))
        using (LogContext.PushProperty("RequestBody", request, destructureObjects: true))
        {
            logger.LogInformation("Starting request {RequestName}", requestName);

            try
            {
                // Execute the next handler in the pipeline
                TResponse result = await next();
                stopwatch.Stop();

                LogResult(result, stopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                // If an exception occurs, stop the stopwatch and log the exception
                stopwatch.Stop();
                LogException(ex, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }

    private void LogResult(TResponse result, long elapsedMilliseconds)
    {
        if (result.IsSuccess)
        {
            logger.LogInformation(
                "Completed request {RequestName} successfully in {ElapsedMilliseconds}ms",
                typeof(TRequest).Name,
                elapsedMilliseconds);
        }
        else
        {
            // Log failed request completion
            // The entire error object is added as a structured property
            using (LogContext.PushProperty("Error", result.Error, destructureObjects: true))
            {
                logger.LogError(
                    "Request {RequestName} failed in {ElapsedMilliseconds}ms. Error details logged.",
                    typeof(TRequest).Name,
                    elapsedMilliseconds);
            }
        }
    }

    private void LogException(Exception ex, long elapsedMilliseconds)
    {
        // Log any unexpected exceptions
        // The entire exception object is passed to the logger
        logger.LogError(
            ex,
            "Request {RequestName} failed with exception in {ElapsedMilliseconds}ms",
            typeof(TRequest).Name,
            elapsedMilliseconds);
    }
}

