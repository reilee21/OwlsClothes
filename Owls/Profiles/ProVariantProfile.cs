using AutoMapper;
using Owls.DTOs;
using Owls.DTOs.Write;
using Owls.Models;

namespace Owls.Profiles
{
    public class ProVariantProfile :Profile
    {
        public ProVariantProfile()
        {
            CreateMap<ProductVariant, ProductVariantRVM>()
                        .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ColorName)); 

            CreateMap<ProductVariant, ProVariant>().ReverseMap();
        }
    }
}
