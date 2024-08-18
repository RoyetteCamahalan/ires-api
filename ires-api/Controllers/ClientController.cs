using ires.AppService.Common;
using ires.Domain.Common;
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
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<ClientViewModel>>
            {
                Data = await _clientService.GetClients(request)
            };
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
            var client = await _clientService.GetClientByName(requestDto.lname, requestDto.fname);
            if (client != null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Client already registered.";
                return BadRequest(serverResponse);
            }
            var result = await _clientService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ClientRequestDto requestDto)
        {
            await _clientService.Update(requestDto);
            return Ok(new ServerResponse<bool>());
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
                serverResponse.message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}