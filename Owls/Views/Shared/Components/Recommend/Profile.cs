using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Owls.DTOs;
using Owls.Repositories.ProductRepos;
using Owls.Services.Firebase;

namespace Owls.Views.Shared.Components.Recommend
{
    public class RecommendViewComponent : ViewComponent
    {
        private readonly IProductRepos productRepos;
        private readonly IMapper mapper;
        private readonly IFirebaseStorage firebaseStorage;
        public RecommendViewComponent(IProductRepos productRepos, IMapper mapper, IFirebaseStorage storage)
        {
            this.productRepos = productRepos;
            this.mapper = mapper;
            this.firebaseStorage = storage;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bs = await productRepos.GetNewestProducts(10);
            var rs = mapper.Map<IEnumerable<ProductBaseInformation>>(bs);
            foreach (var item in rs)
            {
                if (item.Imagethumbnail != null)
                {
                    item.Imagethumbnail = await firebaseStorage.GetImageStreamAsync(item.Imagethumbnail);
                }
            }
            return View(rs);
        }
    }
}
