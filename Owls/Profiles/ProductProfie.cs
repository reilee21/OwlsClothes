using AutoMapper;
using Owls.DTOs;
using Owls.DTOs.Write;
using Owls.Models;

namespace Owls.Profiles
{
	public class ProductProfie : Profile
	{
		public ProductProfie()
		{
			CreateMap<Product, ProductBaseInformation>()
					   .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.ProductVariants.Sum(pv => pv.Quantity)))
					   .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductVariants.Min(pv => pv.SalePrice)))
					   .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.ProductVariants.Max(pv => pv.Promotions.Sum(p => p.DiscountPercentage))));


			CreateMap<Product, ProductReadVM>()
					   .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.Name)))
					   .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductVariants.Min(pv => pv.SalePrice)))
					   .ReverseMap();
			CreateMap<Product, ProductWrite>()
					   .ForMember(dest => dest.ImagesDisplay, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.Name)))
					   .ForMember(dest => dest.Varriants, opt => opt.MapFrom(src => src.ProductVariants)).ReverseMap();





		}
	}
}
