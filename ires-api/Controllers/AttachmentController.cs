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
        private readonly IFileService _fileService;

        public AttachmentController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int typeID, long attachableID, long lotID)
        {
            var serverResponse = new ServerResponse<ICollection<AttachmentViewModel>>();
            var result = await _fileService.getFiles(typeID, attachableID, lotID);
            serverResponse.Data = result;
            return Ok(serverResponse);
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] AttachmentRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<AttachmentViewModel>();
            var attachment = await _fileService.uploadFile(requestDto);
            if (attachment == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = attachment;
            return Ok(serverResponse);
        }
        [HttpPost("deletefile")]
        public async Task<IActionResult> DeleteFileAsync(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            await _fileService.DeleteFile(requestDto.id);
            return Ok(serverResponse);
        }
    }
}
