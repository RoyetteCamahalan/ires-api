namespace ires_api.Models
{
    public class ServerResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int errorCode { get; set; }
    }
}
