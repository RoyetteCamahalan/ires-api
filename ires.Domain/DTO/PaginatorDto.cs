namespace ires.Domain.DTO
{
    public class PaginatorDto<T>
    {
        public PaginatorDto()
        {

        }
        public PaginatorDto(int currentPage)
        {
            this.currentPage = currentPage;
        }
        public IEnumerable<T>? data { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; } = 10;
        public int totalRecord { get; set; }
        public int totalPages { get; set; }
        public string info { get; set; } = string.Empty;
        public void Paginate(IEnumerable<T> rawData)
        {
            totalRecord = rawData.Count();
            totalPages = totalRecord / pageSize + (totalRecord % pageSize > 0 ? 1 : 0);
            if (currentPage > 0)
            {
                data = rawData.Skip((currentPage - 1) * pageSize).Take(pageSize);
                var pageStart = pageSize * (currentPage - 1);
                info = string.Format("Showing {0}-{1}{2}",
                    pageStart + 1, pageStart + pageSize < totalRecord ? pageStart + pageSize : totalRecord,
                    totalRecord <= pageSize ? "" : " of " + totalRecord.ToString());
            }
            else
            {
                data = rawData;
                info = "Showing " + data.Count() + " records";
            }
        }
    }
}
