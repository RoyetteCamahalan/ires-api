using ires.Domain.Contracts;

namespace ires.Domain.Common
{
    public class PaginationRequest : IPaginationInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
    }
}
