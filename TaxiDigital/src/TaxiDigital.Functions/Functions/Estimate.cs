using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TaxiDigital.Functions.Messages;

namespace TaxiDigital.Functions.Functions;

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
        _logger.LogInformation($"Estimate function start processing Message ID: {message.MessageId}");

        var messageBody = message.Body.ToString();
        var estimateMessage = JsonSerializer.Deserialize<EstimateMessage>(messageBody);



        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
