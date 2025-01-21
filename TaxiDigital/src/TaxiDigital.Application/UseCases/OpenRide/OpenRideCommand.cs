using TaxiDigital.Application.Common.Messaging;

namespace TaxiDigital.Application.UseCases.OpenRide;

public sealed record OpenRideCommand(int RideId) : ICommand;