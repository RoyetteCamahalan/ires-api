using ires_api.DTO.Attachment;

namespace ires_api.Services.Interface
{
    public interface IFileService
    {
        public Task<AttachmentViewModel> uploadFile(AttachmentRequestDto requestDto);
        public Task<ICollection<AttachmentViewModel>> getFiles(int typeID, long attachableID, long lotID);
        public Task DeleteFile(long ID);
    }
}
