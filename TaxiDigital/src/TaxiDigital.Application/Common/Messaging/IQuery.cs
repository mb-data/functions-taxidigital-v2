using MediatR;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
