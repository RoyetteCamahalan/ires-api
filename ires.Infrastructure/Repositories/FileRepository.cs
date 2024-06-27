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
                DeleteFile(document.companyid, document.filename);
                await _dataContext.SaveChangesAsync();
            }
        }
        private static void DeleteFile(int companyID, string filename)
        {
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/attachments/" + companyID + "/" + filename);
            if (File.Exists(file))
                File.Delete(file);
        }

        public async Task<ICollection<AttachmentViewModel>> getFiles(int typeID, long attachableID, long lotID)
        {
            var result = await _dataContext.attachments.Where(x => x.typeid == typeID && x.invoiceno == attachableID && x.lotid == lotID && !x.isdeleted).ToListAsync();
            return _mapper.Map<ICollection<AttachmentViewModel>>(result);
        }

        public async Task<AttachmentViewModel> uploadFile(AttachmentRequestDto requestDto)
        {

            if (requestDto.formFile.Length > 0)
            {
                var filename = SaveFile(requestDto);
                if (filename != "")
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
                        filetype = Path.GetExtension(requestDto.formFile.FileName) == ".pdf" ? 1 : 0,
                        filename = filename
                    };
                    var company = await _dataContext.companies.FindAsync(requestDto.companyid);
                    if (requestDto.typeid == -1)
                    {
                        if (company.logo != "")
                            DeleteFile(company.id, company.logo);
                        company.logo = filename;
                    }
                    else
                        _dataContext.attachments.Add(attachment);

                    company.storage += attachment.filesize;
                    await _dataContext.SaveChangesAsync();
                    return _mapper.Map<AttachmentViewModel>(attachment);
                }
            }
            return null;
        }
        private static string SaveFile(AttachmentRequestDto requestDto)
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
                return filename;
            }
            catch (Exception)
            {

            }
            return "";
        }
    }
}
