using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TaxiDigital.Functions.Messages;

namespace TaxiDigital.Functions.Functions;

public class OpenRide(ILogger<OpenRide> logger, ISender sender)
{
    private readonly ILogger<OpenRide> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));

    [Function(nameof(OpenRide))]
    public async Task Run(
        [ServiceBusTrigger("openride-taxidigital", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation($"OpenRide function start processing Message ID: {message.MessageId}");

        var messageBody = message.Body.ToString();
        var openRideMessage = JsonSerializer.Deserialize<OpenRideMessage>(messageBody);



        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
