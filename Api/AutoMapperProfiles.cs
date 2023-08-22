using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Models.DTOs;
using AutoMapper;

namespace Api
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           // CreateMap<GetEmployeeDto, Employee>().ReverseMap();
            CreateMap<Dependent, GetDependentDto>();
            CreateMap<Dependent, CreateDependentRequest>().ReverseMap();
            CreateMap<Employee, GetEmployeeDto>()
                .ForMember(dest => dest.Dependents, opt => opt.MapFrom(src => src.Dependents));

        }
    }
}
