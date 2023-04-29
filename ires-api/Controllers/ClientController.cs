using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private readonly IMailService _mailService;

        public ClientController(IMapper mapper, IClientService clientService, IMailService mailService)
        {
            _mapper = mapper;
            _clientService = clientService;
            _mailService = mailService;
        }
        [HttpGet]
        public IActionResult Get(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<ClientDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            List<ClientDto> clientDtos = _mapper.Map<List<ClientDto>>(_clientService.GetClients(identity.companyid ?? 0, search ?? ""));
            var paginator = new PaginatorDto<ClientDto>(currentPage);
            paginator.Paginate(clientDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(long id)
        {
            var serverResponse = new ServerResponse<ClientDto>();
            var client = _clientService.GetClientById(id);
            if (client == null)
            {
                serverResponse.Success = false;
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<ClientDto>(client);
            return Ok(serverResponse);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClientDto requestDto)
        {
            var serverResponse = new ServerResponse<ClientDto>();
            var client = _clientService.GetClientByName(requestDto.companyid, requestDto.lname, requestDto.fname);
            if (client != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Client already registered.";
                return BadRequest(serverResponse);
            }
            var result = _clientService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<ClientDto>(client);
            return Ok(serverResponse);
        }

        [HttpPut]
        public IActionResult Put([FromBody] ClientDto requestDto)
        {
            var serverResponse = new ServerResponse<ClientDto>();
            var client = _clientService.Update(requestDto);
            if (client == null)
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
            var serverResponse = new ServerResponse<Boolean>();
            serverResponse.Success = _mailService.SendEmail("Message From: " + requestDto.name, new List<string> { _mailService.GetPublicEmail() }, "Message: " + requestDto.message);
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
    }
}
