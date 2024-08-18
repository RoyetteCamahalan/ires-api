namespace ires.Domain.Common
{
    public class PaginatedResult<T>()
    {
        public IEnumerable<T>? data { get; set; }
        public int currentPage { get; set; }
        public int totalRecord { get; set; }
        public int totalPages { get; set; }
        public string info { get; set; } = string.Empty;
    }
}
