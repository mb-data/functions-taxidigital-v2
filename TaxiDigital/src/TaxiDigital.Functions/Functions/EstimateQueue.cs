using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaxiDigital.Application.UseCases.EstimateQueue;
using TaxiDigital.Functions.Messages;

namespace TaxiDigital.Functions.Functions;

public class EstimateQueue(ILogger<EstimateQueue> logger, ISender sender)
{
    private readonly ILogger<EstimateQueue> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));

    [Function(nameof(EstimateQueue))]
    public async Task Run(
        [ServiceBusTrigger("estimate-taxidigital-queue", Connection = "AzureWebJobsStorage")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation($"EstimateQueue function start processing Message ID: {message.MessageId}");

        var messageBody = message.Body.ToString();
        var estimateMessage = JsonSerializer.Deserialize<EstimateQueueMessage>(messageBody);

        foreach (var estimativeQueueRequest in estimateMessage.EstimativeQueueRequests)
        {
            var command = new EstimateQueueCommand(estimativeQueueRequest.RideID, estimativeQueueRequest.ProviderID, estimativeQueueRequest.CompanyConfigurationTypeID);
            var result = await _sender.Send(command);
        }

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
