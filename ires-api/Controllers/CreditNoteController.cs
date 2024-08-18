using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO.CreditNote;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditNoteController(ICreditNoteService _creditMemoService) : ControllerBase
    {

        [HttpGet("getcredittype/{id}")]
        public async Task<IActionResult> GetCreditType(long id)
        {
            var serverResponse = new ServerResponse<CreditMemoTypeViewModel>
            {
                Data = await _creditMemoService.GetType(id)
            };
            return Ok(serverResponse);

        }

        [HttpGet("getcredittypes")]
        public async Task<IActionResult> GetCreditTypes([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<CreditMemoTypeViewModel>>
            {
                Data = await _creditMemoService.GetTypes(request)
            };
            return Ok(serverResponse);

        }
        [HttpPost("createcredittype")]
        public async Task<IActionResult> CreateCreditType([FromBody] CreditMemoTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<CreditMemoTypeViewModel>();
            var bank = await _creditMemoService.GetTypeByName(requestDto.name);
            if (bank != null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Credit Memo type already registered.";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = await _creditMemoService.CreateType(requestDto);
            return Ok(serverResponse);
        }

        [HttpPut("updatecredittype")]
        public async Task<IActionResult> Put([FromBody] CreditMemoTypeRequestDto requestDto)
        {
            ;
            await _creditMemoService.UpdateType(requestDto);
            return Ok(new ServerResponse<bool>());
        }
    }
}