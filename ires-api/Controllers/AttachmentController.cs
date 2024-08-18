using ires.AppService.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Attachment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController(IFileService _fileService) : ControllerBase
    {

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
            if (requestDto.filetype == ires.Domain.Enumerations.FileType.unsupported)
            {
                serverResponse.Success = false;
                serverResponse.message = "Unsupported file format";
                return BadRequest(serverResponse);
            }
            var attachment = await _fileService.uploadFile(requestDto);
            if (attachment == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = attachment;
            return Ok(serverResponse);
        }
        [HttpGet("download")]
        [AllowAnonymous]
        public async Task<IActionResult> Download(string filename)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            if (!System.IO.File.Exists(filePath))
                return BadRequest("File not found!");

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
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
