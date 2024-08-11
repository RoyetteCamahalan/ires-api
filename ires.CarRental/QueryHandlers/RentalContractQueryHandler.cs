using AutoMapper;
using ires.Application.Queries.RentalCharge;
using ires.Application.Queries.RentalContract;
using ires.Application.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using MediatR;

namespace ires.Application.QueryHandlers
{
    public class RentalContractQueryHandler(
        IRentalService _rentalService,
        IMapper _mapper) :
        IRequestHandler<GetRentalContractByIdQuery, RentalContractViewModel>,
        IRequestHandler<GetRentalContractDetailsQuery, IEnumerable<RentalContractDetailViewModel>>,
        IRequestHandler<GetAllRentalContractsQuery, PaginatedResult<RentalContractViewModel>>,
        IRequestHandler<GetRentalAccountHistoryQuery, IEnumerable<RentalHistoryViewModel>>,
        IRequestHandler<GenerateRentalSOAQuery, FileDataViewModel>,
        IRequestHandler<GetRentalChargeByIdQuery, RentalChargeViewModel>
    {
        public async Task<RentalContractViewModel> Handle(GetRentalContractByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _rentalService.Get(request.id);
            var viewModel = _mapper.Map<RentalContractViewModel>(result);
            viewModel.propertyList = await _rentalService.GetPropertiesAsString(result.contractid);
            return viewModel;
        }

        public async Task<IEnumerable<RentalContractDetailViewModel>> Handle(GetRentalContractDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _rentalService.GetDetails(request.id);
            return _mapper.Map<IEnumerable<RentalContractDetailViewModel>>(result);
        }

        public async Task<PaginatedResult<RentalContractViewModel>> Handle(GetAllRentalContractsQuery request, CancellationToken cancellationToken)
        {
            var result = await _rentalService.GetAll(request.data);
            var data = _mapper.Map<PaginatedResult<RentalContractViewModel>>(result);
            foreach (var viewModel in data.data)
                viewModel.propertyList = await _rentalService.GetPropertiesAsString(viewModel.contractid);
            return data;
        }

        public async Task<IEnumerable<RentalHistoryViewModel>> Handle(GetRentalAccountHistoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _rentalService.GetAccountHistory(request.id);
            return _mapper.Map<IEnumerable<RentalHistoryViewModel>>(result);
        }

        public async Task<FileDataViewModel> Handle(GenerateRentalSOAQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<FileDataViewModel>(await _rentalService.GenerateSOA(request.id));
        }

        public async Task<RentalChargeViewModel> Handle(GetRentalChargeByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<RentalChargeViewModel>(await _rentalService.GetRentalCharge(request.id));
        }
    }
}
