using ires_api.Data;
using ires_api.DTO.Attachment;
using ires_api.Models;
using ires_api.Services.Interface;

namespace ires_api.Services.Repository
{
    public class FileRepository : IFileService
    {
        private readonly DataContext _dataContext;

        public FileRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private Attachment GetAttachmentByID(long ID)
        {
            return _dataContext.attachments.Find(ID);
        }
        public void DeleteFile(long ID)
        {
            var document = GetAttachmentByID(ID);
            if (document != null)
            {
                document.isdeleted = true;
                var company = _dataContext.companies.Find(document.companyid);
                company.storage -= document.filesize;
                string filename = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/attachments/" + document.companyid + "/" + document.filename);
                if (File.Exists(filename))
                    File.Delete(filename);
                _dataContext.SaveChanges();
            }
        }

        public ICollection<Attachment> getFiles(int typeID, long attachableID, long lotID)
        {
            return _dataContext.attachments.Where(x => x.typeid == typeID && x.invoiceno == attachableID && x.lotid == lotID && !x.isdeleted).ToList();
        }

        public Attachment uploadFile(AttachmentRequestDto requestDto)
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
                    var company = _dataContext.companies.Find(requestDto.companyid);
                    company.storage += attachment.filesize;
                    _dataContext.SaveChanges();
                    return attachment;
                }
                catch (Exception)
                {

                }
            }
            return null;
        }
    }
}
