using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.CreditNote;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditNoteController : ControllerBase
    {
        private readonly ICreditNoteService _creditMemoService;

        public CreditNoteController(ICreditNoteService creditMemoService)
        {
            _creditMemoService = creditMemoService;
        }
        [HttpGet("getcredittype/{id}")]
        public async Task<IActionResult> GetCreditType(long id)
        {
            var serverResponse = new ServerResponse<CreditMemoTypeViewModel>();
            var result = await _creditMemoService.GetType(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);

        }

        [HttpGet("getcredittypes")]
        public async Task<IActionResult> GetCreditTypes(int currentPage, bool viewAll, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<CreditMemoTypeViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _creditMemoService.GetTypes(identity.companyid ?? 0, search ?? "", viewAll);
            var paginator = new PaginatorDto<CreditMemoTypeViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createcredittype")]
        public async Task<IActionResult> CreateCreditType([FromBody] CreditMemoTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CreditMemoTypeViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var bank = await _creditMemoService.GetTypeByName(requestDto.companyid, requestDto.name);
            if (bank != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Credit Memo type already registered.";
                return BadRequest(serverResponse);
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            var result = await _creditMemoService.CreateType(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut("updatecredittype")]
        public async Task<IActionResult> Put([FromBody] CreditMemoTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _creditMemoService.UpdateType(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}