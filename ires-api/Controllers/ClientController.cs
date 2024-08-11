using AutoMapper;
using ires.Application.Commands.Car;
using ires.Application.Commands.Client;
using ires.Application.Commands.General;
using ires.Application.Queries.Client;
using ires.Application.ViewModels;
using ires.AppService.Dto;
using ires.AppService.Dto.Car;
using ires.AppService.Dto.Client;
using ires.Domain.Common;
using ires_api.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(IMediator _mediator, IMapper _mapper) : BaseController(_mediator, _mapper)
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            return await Handle<PaginatedResult<ClientViewModel>>(new GetAllClientsQuery(request));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            return await Handle<ClientViewModel>(new GetClientByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateClientRequestDto request)
        {
            return await Handle<CreateClientRequestDto, CreateClientCommand, ClientViewModel>(request);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateClientRequestDto request)
        {
            return await Handle<UpdateCarRequestDto, UpdateCarCommand, object>(request);
        }

        [HttpPost("sendmail")]
        [AllowAnonymous]
        public async Task<IActionResult> SendMail([FromBody] SendMailRequestDto request)
        {
            return await Handle<SendMailRequestDto, SendMailInquiryCommand, object>(request);
        }
    }
}
