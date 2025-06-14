using ires.Core.ViewModels;
using ires.Domain.Enumerations;
using MediatR;

namespace ires.Core.Commands.Lot
{
    public record UpdateLotCommand(
        long lot_id,
        int blocknoint,
        int lotnoint,
        string name,
        decimal area,
        decimal pricepersquare,
        decimal default_price,
        decimal compercentage,
        decimal commissionableamount,
        decimal comatdown,
        long model_id,
        string titleno,
        LotStatus status) : IRequest
    {
    }
}
