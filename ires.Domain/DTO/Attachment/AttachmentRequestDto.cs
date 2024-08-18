using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Http;

namespace ires.Domain.DTO.Attachment
{
    public class AttachmentRequestDto
    {
        public long invoiceno { get; set; }
        public long lotid { get; set; }
        public string documentname { get; set; } = string.Empty;
        public int typeid { get; set; }
        public IFormFile? formFile { get; set; }
        public FileType filetype
        {
            get
            {
                if (formFile == null)
                    return FileType.unsupported;
                switch (Path.GetExtension(formFile.FileName))
                {
                    case ".pdf":
                        return FileType.pdf;
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                    case ".raw":
                    case ".bmp":
                    case ".heif":
                    case ".heic":
                    case ".svg":
                        return FileType.img;
                    case ".doc":
                    case ".docx":
                    case ".txt":
                    case ".rtf":
                    case ".wps":
                    case ".xps":
                    case ".dotx":
                        return FileType.doc;
                    case ".xls":
                    case ".xlt":
                    case ".xlm":
                    case ".xlsx":
                    case ".xlsm":
                    case ".xltx":
                    case ".xltm":
                    case ".csv":
                        return FileType.spreadsheet;
                    case ".tif":
                    case ".jp2":
                    case ".ecw":
                    case ".tfw":
                        return FileType.oth;
                }
                return FileType.unsupported;
            }
        }
    }
}
