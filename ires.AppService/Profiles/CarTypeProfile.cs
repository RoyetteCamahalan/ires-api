using AutoMapper;
using ires.Application.ViewModels;
using ires.Domain.Models;
using Entities = ires.Infrastructure.Entities;


namespace ires.AppService.Profiles
{
    public class CarTypeProfile : Profile
    {
        public CarTypeProfile()
        {
            CreateMap<Entities.CarType, CarType>();
            CreateMap<CarType, CarTypeViewModel>();
        }
    }
}
