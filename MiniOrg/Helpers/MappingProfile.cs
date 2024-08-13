namespace TestApi.Helpers;

using AutoMapper;
using TestApi.Dtos;
using TestApi.Models;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Customer, CustomerDto>().ReverseMap();
		CreateMap<Department, DepartmentDto>().ReverseMap();
		CreateMap<Employee, EmployeeDto>().ReverseMap();
		CreateMap<EmployeeReview, EmployeeReviewDto>().ReverseMap();
		CreateMap<Manager, ManagerDto>().ReverseMap();
	}
}
