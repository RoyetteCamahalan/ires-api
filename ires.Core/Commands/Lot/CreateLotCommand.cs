using ires.Core.ViewModels;
using MediatR;

namespace ires.Core.Commands.Lot
{
    public record CreateLotCommand(
        long propertyid,
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
        string titleno) : IRequest<LotViewModel>
    {
    }
}
