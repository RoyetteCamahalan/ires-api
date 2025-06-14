using AutoMapper;
using ires.Core.Commands.Lot;
using ires.Core.ViewModels;
using ires.Domain.Contracts;
using ires.Domain.Models;
using MediatR;

namespace ires.Core.CommandHandlers
{
    public class LotCommandHandler(ILotService _lotService, IMapper _mapper) :
        IRequestHandler<CreateLotCommand, LotViewModel>,
        IRequestHandler<UpdateLotCommand>
    {
        public async Task<Unit> Handle(UpdateLotCommand request, CancellationToken cancellationToken)
        {
            await _lotService.Update(_mapper.Map<Lot>(request));
            return new Unit();
        }

        public async Task<LotViewModel> Handle(CreateLotCommand request, CancellationToken cancellationToken)
        {
            var result = await _lotService.Create(_mapper.Map<Lot>(request));
            return _mapper.Map<LotViewModel>(result);
        }
    }
}
