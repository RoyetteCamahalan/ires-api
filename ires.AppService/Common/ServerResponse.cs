namespace ires.AppService.Common
{
    public class ServerResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string status
        {
            get => Success ? "success" : "error";
        }
        public string message { get; set; } = string.Empty;
        public int errorCode { get; set; }
    }
}
