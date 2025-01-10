using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TaxiDigital.Functions;

public class Estimate(ILogger<Estimate> logger)
{
    private readonly ILogger<Estimate> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [Function(nameof(Estimate))]
    [ServiceBusOutput("estimate-taxidigital-queue", Connection = "ServiceBusConnection")]
    public async Task Run(
        [ServiceBusTrigger("estimate-taxidigital", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
