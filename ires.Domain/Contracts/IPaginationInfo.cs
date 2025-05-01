namespace ires.Domain.Contracts
{
    public interface IPaginationInfo
    {
        string? Search { get; }
        int PageNumber { get; }
        int PageSize { get; }
    }
}
