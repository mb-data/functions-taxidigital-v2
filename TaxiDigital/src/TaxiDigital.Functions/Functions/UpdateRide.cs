using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaxiDigital.Application.UseCases.UpdateRide;
using TaxiDigital.Functions.Messages;

namespace TaxiDigital.Functions.Functions
{
    public class UpdateRide(ILogger<UpdateRide> logger, ISender sender)
    {
        private readonly ILogger<UpdateRide> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));

        [Function(nameof(UpdateRide))]
        public async Task Run(
            [ServiceBusTrigger("post", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation($"UpdateRide function start processing Message ID: {message.MessageId}");

            var messageBody = message.Body.ToString();
            var updateRideMessage = JsonSerializer.Deserialize<UpdateRideMessage>(messageBody);

            var command = new UpdateRideCommand(updateRideMessage.Type, updateRideMessage.BookingId, updateRideMessage.ExternalAuthorizationId);
            var result = await _sender.Send(command);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
