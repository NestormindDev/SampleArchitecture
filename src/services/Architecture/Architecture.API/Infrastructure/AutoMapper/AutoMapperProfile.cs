using AutoMapper;
using Architecture.API.Application.Models;
using Architecture.Domain.AggregatesModel;

namespace Architecture.API.Infrastructure.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
              
            CreateMap<CountryDto, Country>().ReverseMap(); 
        }
    }
}
