namespace ires.Domain.Contracts
{
    public interface ICurrentUserService
    {
        long employeeid { get; }
        int companyid { get; }
    }
}
