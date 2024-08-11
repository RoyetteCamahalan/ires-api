using AutoMapper;
using ires.Application.Commands.General;
using ires.Application.Commands.RentalContract;
using ires.Application.ViewModels;
using ires.Domain.Contracts;
using ires.Domain.Models;
using MediatR;

namespace ires.Application.CommandHandlers
{
    public class RentalContractCommandHandler(IRentalService _rentalService, IMapper _mapper) :
        IRequestHandler<CreateRentalContractCommand, RentalContractViewModel>,
        IRequestHandler<UpdateRentalContractCommand>,
        IRequestHandler<ReComputeRentalContractCommand>,
        IRequestHandler<SendRentalSOACommand>,
        IRequestHandler<TerminateRentalContractCommand>
    {
        public async Task<Unit> Handle(UpdateRentalContractCommand request, CancellationToken cancellationToken)
        {
            await _rentalService.Update(_mapper.Map<RentalContract>(request));
            return new Unit();
        }

        public async Task<RentalContractViewModel> Handle(CreateRentalContractCommand request, CancellationToken cancellationToken)
        {
            var result = await _rentalService.Create(_mapper.Map<RentalContract>(request));
            return _mapper.Map<RentalContractViewModel>(result);
        }
        public async Task<Unit> Handle(SendRentalSOACommand request, CancellationToken cancellationToken)
        {
            await _rentalService.SendSOA(_mapper.Map<MailingInfo>(request));
            return new Unit();
        }

        public async Task<Unit> Handle(ReComputeRentalContractCommand request, CancellationToken cancellationToken)
        {
            await _rentalService.RecomputeContract(request.id);
            return new Unit();
        }

        public async Task<Unit> Handle(TerminateRentalContractCommand request, CancellationToken cancellationToken)
        {
            await _rentalService.UpdateContractStatus(_mapper.Map<RentalContract>(request));
            return new Unit();
        }
    }
}
