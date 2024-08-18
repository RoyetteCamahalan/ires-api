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
            this.currentPage = PageNumber;
            this.search = Search;
        }
        public int currentPage { get; set; }
        public int PageSize { get; set; } = 10;
        public string? search { get; set; }
        public string searchString { get => search ?? ""; }
        public int filterBy { get; set; }
        public bool viewAll { get; set; }
        public bool isEWallet { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public long projectID { get; set; }
    }
}
