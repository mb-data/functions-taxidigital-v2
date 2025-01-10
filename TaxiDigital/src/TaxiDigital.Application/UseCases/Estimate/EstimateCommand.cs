using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.Domain.Estimate.Requests;

namespace TaxiDigital.Application.UseCases.Estimate;

public sealed record EstimateCommand(int RideId) : ICommand<List<EstimativeQueueRequest>>;
