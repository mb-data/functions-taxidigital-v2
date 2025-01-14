using TaxiDigital.Application.Common.Messaging;

namespace TaxiDigital.Application.UseCases.EstimateQueue;

public sealed record EstimateQueueCommand(int RideID, int ProviderID, string CompanyConfigurationTypeID) : ICommand;