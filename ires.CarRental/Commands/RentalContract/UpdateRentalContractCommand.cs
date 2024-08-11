using ires.Application.Commands.RentalContractDetail;
using MediatR;

namespace ires.Application.Commands.RentalContract
{
    public record UpdateRentalContractCommand(
        long contractid,
        long contractno,
        long custid,
        DateTime contractdate,
        decimal montlyrent,
        decimal? deposit,
        int? term,
        int? noofmonthadvance,
        decimal ewtpercentage,
        decimal? monthlypenalty,
        int? penaltyextension,
        int billingsched,
        string remarks,
        List<RentalContractDetailCommand> rentalContractDetails) : IRequest
    {
    }
}
