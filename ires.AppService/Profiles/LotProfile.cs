using AutoMapper;
using ires.Core.ViewModels;
using ires.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.AppService.Profiles
{
    public class LotProfile : Profile
    {
        public LotProfile()
        {
            CreateMap<Infrastructure.Entities.Lot, Lot>().ReverseMap();
            CreateMap<Lot, LotViewModel>();
        }
    }
}
