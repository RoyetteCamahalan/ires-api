using AutoMapper;
using ires.AppService.DTO.Project;
using ires.Core.Commands.Project;
using ires.Core.ViewModels;
using ires.Domain.Models;

namespace ires.AppService.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<RentalProjectRequestDto, Project>();

            CreateMap<CreateProjectRequestDto, CreateProjectCommand>();
            CreateMap<CreateProjectCommand, Project>();
            CreateMap<UpdateProjectRequestDto, UpdateProjectCommand>();
            CreateMap<UpdateProjectCommand, Project>();
            CreateMap<Infrastructure.Entities.Project, Project>().ReverseMap();
            CreateMap<Project, RentalProjectViewModel>();
            CreateMap<Project, ProjectViewModel>();
        }
    }
}
