using AutoMapper;
using Owls.DTOs.Write;
using Owls.Models;

namespace Owls.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<OwlsUser, AccountWrite>().ReverseMap();
        }
    }
}
