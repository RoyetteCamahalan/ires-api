using AutoMapper;
using ires.AppService.DTO.LotModel;
using ires.Core.Commands.LotModel;
using ires.Core.Queries.LotModel;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires_api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotModelController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] long project_id, [FromQuery] PaginationRequest request)
        {
            return await Handle<PaginatedResult<LotModelViewModel>>(new GetLotModelsQuery(project_id, request));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return await Handle<LotModelViewModel>(new GetLotModelbyIdQuery(id));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateLotModelRequestDto request)
        {
            return await Handle<CreateLotModelRequestDto, CreateLotModelCommand, LotModelViewModel>(request);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateLotModelRequestDto request)
        {
            return await Handle<UpdateLotModelRequestDto, UpdateLotModelCommand, object>(request);
        }
    }
}
