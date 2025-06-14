using AutoMapper;
using ires.Core.Queries.Lot;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using MediatR;

namespace ires.Core.QueryHandlers
{
    public class LotQueryHandler(ILotService _lotService, IMapper _mapper) :
        IRequestHandler<GetLotsQuery, PaginatedResult<LotViewModel>>,
        IRequestHandler<GetLotbyIdQuery, LotViewModel>,
        IRequestHandler<GetAvailableLotsQuery, PaginatedResult<LotViewModel>>
    {
        public async Task<PaginatedResult<LotViewModel>> Handle(GetLotsQuery request, CancellationToken cancellationToken)
        {
            var data = await _lotService.GetLotsByProjectId(request.projectId, request.data);
            return _mapper.Map<PaginatedResult<LotViewModel>>(data);
        }

        public async Task<LotViewModel> Handle(GetLotbyIdQuery request, CancellationToken cancellationToken)
        {
            var data =  await _lotService.GetLotById(request.id);
            return _mapper.Map<LotViewModel>(data);
        }

        public async Task<PaginatedResult<LotViewModel>> Handle(GetAvailableLotsQuery request, CancellationToken cancellationToken)
        {
            var data = await _lotService.GetAvailableLotByProject(request.project_guid, request.data);
            return _mapper.Map<PaginatedResult<LotViewModel>>(data);
        }
    }
}
