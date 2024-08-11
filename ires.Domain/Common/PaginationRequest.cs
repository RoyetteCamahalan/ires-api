using ires.Domain.Contracts;

namespace ires.Domain.Common
{
    public class PaginationRequest : IPaginationInfo
    {
        public PaginationRequest()
        {

        }
        public PaginationRequest(int PageNumber, string Search)
        {
            this.PageNumber = PageNumber;
            this.Search = Search;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = string.Empty;
        public int filterByID { get; set; }
    }
}
