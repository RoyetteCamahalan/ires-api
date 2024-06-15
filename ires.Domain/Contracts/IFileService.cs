using ires.Domain.DTO.Attachment;

namespace ires.Domain.Contracts
{
    public interface IFileService
    {
        public Task<AttachmentViewModel> uploadFile(AttachmentRequestDto requestDto);
        public Task<ICollection<AttachmentViewModel>> getFiles(int typeID, long attachableID, long lotID);
        public Task DeleteFile(long ID);
    }
}
