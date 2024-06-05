using ires_api.DTO.Attachment;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IFileService
    {
        public Attachment uploadFile(AttachmentRequestDto requestDto);
        public ICollection<Attachment> getFiles(int typeID, long attachableID, long lotID);
        public void DeleteFile(long ID);
    }
}
