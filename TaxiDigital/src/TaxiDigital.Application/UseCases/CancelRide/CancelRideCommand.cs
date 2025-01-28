using TaxiDigital.Application.Common.Messaging;

namespace TaxiDigital.Application.UseCases.CancelRide;

public sealed record CancelRideCommand(int RideId) : ICommand;
