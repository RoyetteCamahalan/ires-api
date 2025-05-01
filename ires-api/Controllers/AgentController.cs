using AutoMapper;
using ires.AppService.DTO.Agent;
using ires.Core.Commands.Agent;
using ires.Core.Queries.Agent;
using ires.Core.ViewModels;
using ires.Domain.Common;
using ires.Domain.DTO.Employee;
using ires_api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            return await Handle<PaginatedResult<AgentViewModel>>(new GetAgentsQuery(request));
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            return await Handle<AgentViewModel>(new GetAgentbyGuidQuery(guid));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAgentRequestDto requestDto)
        {
            return await Handle<CreateAgentRequestDto, CreateAgentCommand, AgentViewModel>(requestDto);
        }

        // PUT api/<AgentController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateAgentRequestDto requestDto)
        {
            return await Handle<UpdateAgentRequestDto, UpdateAgentCommand, object>(requestDto);
        }
    }
}
