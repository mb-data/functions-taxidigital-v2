using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.UseCases.UpdateRide;

internal sealed class UpdateRideCommandHandler : ICommandHandler<UpdateRideCommand>
{
    public Task<Result> Handle(UpdateRideCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
