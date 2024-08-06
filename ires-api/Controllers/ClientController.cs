using ires.AppService.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(IClientService _clientService, IMailService _mailService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<ClientViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _clientService.GetClients(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<ClientViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<ClientViewModel>();
            var client = await _clientService.GetByID(id);
            if (client == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = client;
            return Ok(serverResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ClientViewModel>();
            var client = await _clientService.GetClientByName(requestDto.companyid, requestDto.lname, requestDto.fname);
            if (client != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Client already registered.";
                return BadRequest(serverResponse);
            }
            var result = await _clientService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ClientRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ClientViewModel>();
            if (!await _clientService.Update(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPost("sendmail")]
        [AllowAnonymous]
        public IActionResult SendMail([FromBody] SendMailRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<Boolean>
            {
                Success = _mailService.SendEmailAsync("Message From: " + requestDto.name, [_mailService.GetPublicEmail()], "Email: " + requestDto.email + " Message: " + requestDto.message)
            };
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
