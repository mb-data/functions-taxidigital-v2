using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaxiDigital.Application.UseCases.Estimate;
using TaxiDigital.Domain.Estimate.Requests;
using TaxiDigital.Functions.Messages;

namespace TaxiDigital.Functions.Functions;

public class Estimate(ILogger<Estimate> logger, ISender sender)
{
    private readonly ILogger<Estimate> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));

    [Function(nameof(Estimate))]
    [ServiceBusOutput("estimate-taxidigital-queue", Connection = "ServiceBusConnection")]
    public async Task<List<EstimativeQueueRequest>> Run(
        [ServiceBusTrigger("estimate-taxidigital", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation($"Estimate function start processing Message ID: {message.MessageId}");

        var messageBody = message.Body.ToString();
        var estimateMessage = JsonSerializer.Deserialize<EstimateMessage>(messageBody);

        var command = new EstimateCommand(estimateMessage.RideId);
        var result = await _sender.Send(command);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
        return result.Value;
    }
}
