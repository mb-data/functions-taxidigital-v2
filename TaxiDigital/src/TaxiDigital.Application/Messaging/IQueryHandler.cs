using MediatR;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
