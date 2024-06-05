using AutoMapper;
using ires_api.DTO;
using ires_api.DTO.Attachment;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AttachmentController(IMapper mapper, IFileService fileService)
        {
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Get(int typeID, long attachableID, long lotID)
        {
            var serverResponse = new ServerResponse<List<AttachmentDto>>();
            List<AttachmentDto> attachmentDtos = _mapper.Map<List<AttachmentDto>>(_fileService.getFiles(typeID, attachableID, lotID));
            serverResponse.Data = attachmentDtos;
            return Ok(serverResponse);
        }
        [HttpPost]
        public IActionResult Post([FromForm] AttachmentRequestDto requestDto)
        {
            ServerResponse<AttachmentDto> serverResponse = new ServerResponse<AttachmentDto>();
            var attachment = _fileService.uploadFile(requestDto);
            if (attachment == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<AttachmentDto>(attachment);
            return Ok(serverResponse);
        }
        [HttpPost("deletefile")]
        public IActionResult DeleteFile(IDRequestDto requestDto)
        {
            ServerResponse<AttachmentDto> serverResponse = new ServerResponse<AttachmentDto>();
            _fileService.DeleteFile(requestDto.id);
            return Ok(serverResponse);
        }
    }
}
