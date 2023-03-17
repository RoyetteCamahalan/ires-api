using AutoMapper;
using ires_api.DTO;
using ires_api.Models;

namespace ires_api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {

            CreateMap<Company, CompanyDto>();

            CreateMap<Employee, EmployeeDto>();

            CreateMap<Employee, EmployeeRequestDto>().ReverseMap();

            CreateMap<Employee, UserLoginDto>()
                .ForMember(dest => dest.company, 
                opts => opts.MapFrom(src =>
                src.company));

            CreateMap<Project, ProjectRequestDto>()
                .ReverseMap();

            CreateMap<UserPrivilege, UserPrivilegeDto>();

            CreateMap<Client, ClientDto>().ReverseMap();

            CreateMap<Survey, SurveyDto>();
            CreateMap<Survey, SurveyRequestDto>().ReverseMap();

            CreateMap<OtherCharge, OtherChargeDto>();
            CreateMap<OtherCharge, OtherChargeRequestDto>().ReverseMap();
        }
    }
}
