using AutoMapper;
using Owls.DTOs;
using Owls.DTOs.Write;
using Owls.Models;

namespace Owls.Profiles
{
	public class ProVariantProfile : Profile
	{
		public ProVariantProfile()
		{
			CreateMap<ProductVariant, ProductVariantRVM>()
						.ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ColorName))
						.ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.Promotions.Sum(p => p.DiscountPercentage)));

			CreateMap<ProductVariant, ProVariant>().ReverseMap();
		}
	}
}
