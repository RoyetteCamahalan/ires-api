using AutoMapper;
using ires.AppService.DTO.RentalUnit;
using ires.Domain.DTO.RentalUnit;
using ires.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.AppService.Profiles
{
    public class RentalPropertyProfile : Profile
    {
        public RentalPropertyProfile()
        {
            CreateMap<RentalUnitRequestDto, RentalUnit>();
            CreateMap<Infrastructure.Entities.RentalProperty, RentalUnit>();
            CreateMap<RentalUnit, Infrastructure.Entities.RentalProperty>().ForMember(x => x.status, opt => opt.Ignore());
            CreateMap<RentalUnit, RentalUnitViewModel>();
        }
    }
}
