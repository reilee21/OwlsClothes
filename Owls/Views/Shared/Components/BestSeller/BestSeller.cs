using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Owls.DTOs;
using Owls.Repositories.ProductRepos;
using Owls.Services.Firebase;

namespace Owls.Views.Shared.Components.BestSeller
{
    public class BestSellerViewComponent : ViewComponent
    {
        private readonly IProductRepos productRepos;
        private readonly IMapper mapper;
        private readonly IFirebaseStorage firebaseStorage;

        public BestSellerViewComponent(IProductRepos productRepos, IMapper mapper, IFirebaseStorage firebaseStorage)
        {
            this.productRepos = productRepos;
            this.mapper = mapper;
            this.firebaseStorage = firebaseStorage;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pros = await productRepos.GetBestSellsProducts(8);
            var proVM = mapper.Map<IEnumerable<ProductBaseInformation>>(pros);

            foreach (var item in proVM)
            {
                if (item.Imagethumbnail != null)
                {
                    item.Imagethumbnail = await firebaseStorage.GetImageStreamAsync(item.Imagethumbnail);
                }
            }
            return View("Default", proVM);
        }
    }
}
