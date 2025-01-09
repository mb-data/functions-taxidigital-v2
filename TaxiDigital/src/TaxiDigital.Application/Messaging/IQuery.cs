using MediatR;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
