using MediatR;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.Common.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;
