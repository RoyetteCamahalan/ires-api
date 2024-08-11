using AutoMapper;
using ires.Application.Commands.General;
using ires.Application.Commands.RentalCharge;
using ires.Application.Commands.RentalContract;
using ires.Application.Queries.RentalCharge;
using ires.Application.Queries.RentalContract;
using ires.Application.ViewModels;
using ires.AppService.Dto.RentalCharge;
using ires.AppService.Dto.RentalContract;
using ires.Domain.Common;
using ires.Domain.DTO.RentalContract;
using ires_api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController(IMediator _mediator, IMapper _mapper) : BaseController(_mediator, _mapper)
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentalContractRequestDto request)
        {
            return await Handle<CreateRentalContractRequestDto, CreateRentalContractCommand, RentalContractViewModel>(request);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRentalContractRequestDto request)
        {
            return await Handle<UpdateRentalContractRequestDto, UpdateRentalContractCommand, object>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return await Handle<RentalContractViewModel>(new GetRentalContractByIdQuery(id));
        }

        [HttpGet("getdetails/{id}")]
        public async Task<IActionResult> GetContractDetails(long id)
        {
            return await Handle<IEnumerable<RentalContractDetailViewModel>>(new GetRentalContractDetailsQuery(id));
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            return await Handle<PaginatedResult<ClientViewModel>>(new GetAllRentalContractsQuery(request));

        }
        [HttpGet("recompute/{id}")]
        public async Task<IActionResult> RecomputeContract(long id)
        {
            return await Handle<object>(new ReComputeRentalContractCommand(id));
        }
        [HttpGet("getaccounthistory/{id}")]
        public async Task<IActionResult> GetAccountHistory(long id)
        {
            return await Handle<IEnumerable<RentalHistoryViewModel>>(new GetRentalAccountHistoryQuery(id));
        }
        [HttpGet("getsoa/{id}")]
        public async Task<IActionResult> GetInvoice(long id)
        {
            return await Handle<FileDataViewModel>(new GenerateRentalSOAQuery(id));
        }


        [HttpGet("getrentalcharge/{id}")]
        public async Task<IActionResult> GetRentalCharge(long id)
        {
            return await Handle<RentalChargeViewModel>(new GetRentalChargeByIdQuery(id));
        }
        [HttpPost("postothercharge")]
        public async Task<IActionResult> PostOtherCharge([FromBody] CreateRentalChargeRequestDto request)
        {
            return await Handle<CreateRentalChargeRequestDto, CreateRentalChargeCommand, RentalChargeViewModel>(request);
        }
        [HttpPut("updateothercharge")]
        public async Task<IActionResult> UpdateOtherCharge([FromBody] UpdateRentalChargeRequestDto request)
        {
            return await Handle<UpdateRentalChargeRequestDto, UpdateRentalChargeCommand, object>(request);
        }
        [HttpDelete("deleteothercharge/{id}")]
        public async Task<IActionResult> DeleteOtherCharge(long id)
        {
            return await Handle<DeleteRentalChargeCommand>(new DeleteRentalChargeCommand(id));
        }
        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus(TerminateRentalContractRequestDto request)
        {
            return await Handle<TerminateRentalContractRequestDto, TerminateRentalContractCommand, object>(request); ;
        }

        [HttpPost("sendsoa")]
        public async Task<IActionResult> SendSoa(SendRentalSOARequestDto request)
        {
            return await Handle<SendRentalSOARequestDto, SendRentalSOACommand, object>(request);
        }
    }
}
