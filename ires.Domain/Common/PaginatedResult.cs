namespace ires.Domain.Common
{
    public class PaginatedResult<T>()
    {
        public IEnumerable<T>? Data { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPages { get; set; }
    }
}
