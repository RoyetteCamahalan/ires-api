using AutoMapper;
using ires.AppService.DTO.Lot;
using ires.Core.Commands.Lot;
using ires.Core.Queries.Lot;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires_api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] long project_id, [FromQuery] PaginationRequest request)
        {
            return await Handle<PaginatedResult<LotViewModel>>(new GetLotsQuery(project_id, request));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return await Handle<LotViewModel>(new GetLotbyIdQuery(id));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateLotRequestDto request)
        {
            return await Handle<CreateLotRequestDto, CreateLotCommand, LotViewModel>(request);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateLotRequestDto request)
        {
            return await Handle<UpdateLotRequestDto, UpdateLotCommand, object>(request);
        }
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable([FromQuery] Guid project_guid, [FromQuery] PaginationRequest request)
        {
            return await Handle<PaginatedResult<LotViewModel>>(new GetAvailableLotsQuery(project_guid, request));
        }
    }
}
