using AutoMapper;
using ires.Application.Commands.RentalCharge;
using ires.Application.ViewModels;
using ires.Domain.Contracts;
using ires.Domain.Models;
using MediatR;

namespace ires.Application.CommandHandlers
{
    public class RentalChargeCommandHandler(
        IRentalService _rentalService,
        IMapper _mapper) :
        IRequestHandler<CreateRentalChargeCommand, RentalChargeViewModel>,
        IRequestHandler<UpdateRentalChargeCommand>,
        IRequestHandler<DeleteRentalChargeCommand>
    {
        public async Task<RentalChargeViewModel> Handle(CreateRentalChargeCommand request, CancellationToken cancellationToken)
        {
            var result = await _rentalService.CreateOtherCharge(_mapper.Map<RentalCharge>(request));
            return _mapper.Map<RentalChargeViewModel>(result);
        }

        public async Task<Unit> Handle(UpdateRentalChargeCommand request, CancellationToken cancellationToken)
        {
            await _rentalService.UpdateOtherCharge(_mapper.Map<RentalCharge>(request));
            return new Unit();
        }

        public async Task<Unit> Handle(DeleteRentalChargeCommand request, CancellationToken cancellationToken)
        {
            await _rentalService.DeleteOtherCharge(request.chargeid);
            return new Unit();
        }
    }
}
