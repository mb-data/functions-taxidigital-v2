using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.UseCases.EstimateQueue;

internal sealed class EstimateQueueCommandHandler : ICommandHandler<EstimateQueueCommand>
{
    public Task<Result> Handle(EstimateQueueCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
