using AutoMapper;
using ires.Core.Commands.LotModel;
using ires.Core.Queries.Agent;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.CommandHandlers
{
    public class LotModelCommandHandler(ILotModelService _lotModelService, IMapper _mapper) :
        IRequestHandler<CreateLotModelCommand, LotModelViewModel>,
        IRequestHandler<UpdateLotModelCommand>
    {
        public async Task<Unit> Handle(UpdateLotModelCommand request, CancellationToken cancellationToken)
        {
            await _lotModelService.Update(_mapper.Map<LotModel>(request));
            return new Unit();
        }

        public async Task<LotModelViewModel> Handle(CreateLotModelCommand request, CancellationToken cancellationToken)
        {
            var result = await _lotModelService.Create(_mapper.Map<LotModel>(request));
            return _mapper.Map<LotModelViewModel>(result);
        }
    }
}
