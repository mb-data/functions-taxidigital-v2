using TaxiDigital.Application.Common.Messaging;

namespace TaxiDigital.Application.UseCases.UpdateRide;

public sealed record UpdateRideCommand(string Type, int BookingId, string ExternalAuthorizationId) : ICommand;