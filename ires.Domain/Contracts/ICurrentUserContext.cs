namespace ires.Domain.Contracts
{
    public interface ICurrentUserContext
    {
        long employeeid { get; }
        int companyid { get; }
    }
}
