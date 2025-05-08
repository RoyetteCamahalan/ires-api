using AutoMapper;
using ires.AppService.DTO.LotModel;
using ires.Core.Commands.LotModel;
using ires.Core.ViewModels;
using ires.Domain.Models;

namespace ires.AppService.Profiles
{
    public class LotModelProfile : Profile
    {
        public LotModelProfile()
        {
            CreateMap<CreateLotModelRequestDto, CreateLotModelCommand>();
            CreateMap<CreateLotModelCommand, LotModel>();
            CreateMap<UpdateLotModelRequestDto,UpdateLotModelCommand>();
            CreateMap<UpdateLotModelCommand, LotModel>();
            CreateMap<Infrastructure.Entities.LotModel, LotModel>().ReverseMap();
            CreateMap<LotModel, LotModelViewModel>();
        }
    }
}
