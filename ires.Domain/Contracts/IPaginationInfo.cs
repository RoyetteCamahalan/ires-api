namespace ires.Domain.Contracts
{
    public interface IPaginationInfo
    {
        int currentPage { get; }
        int PageSize { get; }
    }
}
