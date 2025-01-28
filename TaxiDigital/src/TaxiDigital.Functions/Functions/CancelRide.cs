using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaxiDigital.Application.UseCases.CancelRide;
using TaxiDigital.Functions.Messages;

namespace TaxiDigital.Functions.Functions
{
    public class CancelRide(ILogger<CancelRide> logger, ISender sender)
    {
        private readonly ILogger<CancelRide> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));

        [Function(nameof(CancelRide))]
        public async Task Run(
            [ServiceBusTrigger("cancel-taxidigital", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation($"CancelRide function start processing Message ID: {message.MessageId}");

            var messageBody = message.Body.ToString();
            var cancelRideMessage = JsonSerializer.Deserialize<CancelRideMessage>(messageBody);

            var command = new CancelRideCommand(cancelRideMessage.RideId);
            var result = await _sender.Send(command);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
