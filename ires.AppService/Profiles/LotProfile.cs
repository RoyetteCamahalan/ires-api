using AutoMapper;
using ires.AppService.DTO.Lot;
using ires.Core.Commands.Lot;
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
            CreateMap<CreateLotRequestDto, CreateLotCommand>()
                .ForMember(dest => dest.blocknoint, opt => opt.MapFrom(src => src.blocknoint ?? 0))
                .ForMember(dest => dest.lotnoint, opt => opt.MapFrom(src => src.lotnoint ?? 0));
            CreateMap<CreateLotCommand, Lot>();
            CreateMap<UpdateLotRequestDto, UpdateLotCommand>()
                .ForMember(dest => dest.blocknoint, opt => opt.MapFrom(src => src.blocknoint ?? 0))
                .ForMember(dest => dest.lotnoint, opt => opt.MapFrom(src => src.lotnoint ?? 0));
            CreateMap<UpdateLotCommand, Lot>();
            CreateMap<Infrastructure.Entities.Lot, Lot>().ReverseMap();
            CreateMap<Lot, LotViewModel>();
        }
    }
}
