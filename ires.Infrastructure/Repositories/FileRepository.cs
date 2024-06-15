using AutoMapper;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO.Attachment;
using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Repositories
{
    public class FileRepository : IFileService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public FileRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        private Attachment GetAttachmentByID(long ID)
        {
            return _dataContext.attachments.Find(ID);
        }
        public async Task DeleteFile(long ID)
        {
            var document = GetAttachmentByID(ID);
            if (document != null)
            {
                document.isdeleted = true;
                var company = await _dataContext.companies.FindAsync(document.companyid);
                company.storage -= document.filesize;
                string filename = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/attachments/" + document.companyid + "/" + document.filename);
                if (File.Exists(filename))
                    File.Delete(filename);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<ICollection<AttachmentViewModel>> getFiles(int typeID, long attachableID, long lotID)
        {
            var result = await _dataContext.attachments.Where(x => x.typeid == typeID && x.invoiceno == attachableID && x.lotid == lotID && !x.isdeleted).ToListAsync();
            return _mapper.Map<ICollection<AttachmentViewModel>>(result);
        }

        public async Task<AttachmentViewModel> uploadFile(AttachmentRequestDto requestDto)
        {
            Attachment attachment = new Attachment
            {
                companyid = requestDto.companyid,
                invoiceno = requestDto.invoiceno,
                lotid = requestDto.lotid,
                documentname = Utility.CleanFileName(requestDto.formFile.FileName),
                filesize = Convert.ToDecimal(requestDto.formFile.Length) / 1000000,
                attachedby = requestDto.attachedby,
                dateattached = DateTime.Now,
                isdeleted = false,
                typeid = requestDto.typeid,
                filetype = Path.GetExtension(requestDto.formFile.FileName) == ".pdf" ? 1 : 0
            };

            if (requestDto.formFile.Length > 0)
            {
                try
                {
                    string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/attachments/" + requestDto.companyid);
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);
                    string filename = Utility.RandomString(20) + Path.GetExtension(requestDto.formFile.FileName);
                    string filePath = Path.Combine(uploads, filename);
                    while (File.Exists(filePath))
                    {
                        filename = Utility.RandomString(20) + Path.GetExtension(requestDto.formFile.FileName);
                        filePath = Path.Combine(uploads, filename);
                    }
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        requestDto.formFile.CopyTo(fileStream);
                        fileStream.Close();
                    }
                    attachment.filename = filename;
                    _dataContext.attachments.Add(attachment);
                    var company = await _dataContext.companies.FindAsync(requestDto.companyid);
                    company.storage += attachment.filesize;
                    await _dataContext.SaveChangesAsync();
                    return _mapper.Map<AttachmentViewModel>(attachment);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }
    }
}
