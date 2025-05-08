using AutoMapper;
using ires.Core.Queries.LotModel;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using MediatR;

namespace ires.Core.QueryHandlers
{
    public class LotModelQueryHandler(ILotModelService _lotModelService, IMapper _mapper) :
        IRequestHandler<GetLotModelsQuery, PaginatedResult<LotModelViewModel>>,
        IRequestHandler<GetLotModelbyIdQuery, LotModelViewModel>
    {
        public async Task<PaginatedResult<LotModelViewModel>> Handle(GetLotModelsQuery request, CancellationToken cancellationToken)
        {
            var data = await _lotModelService.GetLotModelByProjectId(request.projectId, request.data);
            return _mapper.Map<PaginatedResult<LotModelViewModel>>(data);
        }

        public async Task<LotModelViewModel> Handle(GetLotModelbyIdQuery request, CancellationToken cancellationToken)
        {
            var data =  await _lotModelService.GetLotModelById(request.id);
            return _mapper.Map<LotModelViewModel>(data);
        }
    }
}
