using ires.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.Commands.Project
{
    public record UpdateProjectCommand(
        long propertyid,
        string propertyname,
        string address,
        string alias,
        decimal area,
        int computationtype,
        decimal defaultcommission,
        decimal com_percentage,
        decimal compercentageoverterm,
        int paymentterm,
        decimal interest,
        int commissionterm,
        int paymentextension,
        int allow_straight_monthly,
        decimal withholding,
        int interesttype,
        decimal addoninterestpermonth) : IRequest
    {
    }
}
