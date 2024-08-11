namespace ires.Domain.Models
{
    public class FileData
    {
        public string filename { get; set; } = string.Empty;
        public string filepath { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;

        public string fullpath
        {
            get
            {
                return filepath + "/" + filename;
            }
        }
    }
}
