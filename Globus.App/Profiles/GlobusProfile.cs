using AutoMapper;
using Globus.App.Data.Entities;
using Globus.App.DTO;

namespace Globus.App.Profiles
{
    public class GlobusProfile : Profile
    {
        public GlobusProfile()
        {
            CreateMap<Customer, CustomerResponse>();
        }
    }
}
