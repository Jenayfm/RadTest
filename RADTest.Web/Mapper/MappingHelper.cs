using AutoMapper;
using RADTest.Domain.Entities;
using RADTest.Models;

namespace RADTest.Web.Mapper;

public class MappingHelper : Profile
{
    public MappingHelper()
    {
        CreateMap<Account, AccountDto>();
    }
}