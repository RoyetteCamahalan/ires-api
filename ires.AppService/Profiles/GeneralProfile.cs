using AutoMapper;
using ires.Application.ViewModels;
using ires.Domain.Models;

namespace ires.AppService.Profiles
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<FileData, FileDataViewModel>();
        }
    }
}
