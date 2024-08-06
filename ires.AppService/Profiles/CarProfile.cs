using AutoMapper;
using ires.AppService.Dto.Car;
using ires.CarRental.Commands.Car;
using ires.CarRental.ViewModels;
using ires.Domain.Models;
using Entities = ires.Infrastructure.Entities;

namespace ires.AppService.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<CreateCarRequestDto, CreateCarCommand>();
            CreateMap<CreateCarCommand, Car>();
            CreateMap<UpdateCarRequestDto, UpdateCarCommand>();
            CreateMap<UpdateCarCommand, Car>();
            CreateMap<Car, CarViewModel>();
            CreateMap<Car, Entities.Car>().ReverseMap();
        }
    }
}
