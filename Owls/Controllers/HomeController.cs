using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Owls.DTOs;
using Owls.Repositories.ProductRepos;
using Owls.Services.Firebase;

namespace Owls.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepos productRepos;
        private readonly IFirebaseStorage firebaseStorage;
        private readonly IMapper mapper;

        public HomeController(IProductRepos productRepos, IFirebaseStorage firebase, IMapper mp)
        {
            this.productRepos = productRepos;
            this.firebaseStorage = firebase;
            this.mapper = mp;

        }

        public async Task<IActionResult> Index()
        {
            List<string> imageUrls = new List<string> { "/image/banner-1.png", "/image/banner-2.png", "/image/banner-3.png", "/image/banner-4.png", "/image/banner-5.png" };
            ViewBag.ImageUrls = imageUrls;


            var pros = await productRepos.GetNewestProducts(8);
            var proVM = mapper.Map<IEnumerable<ProductBaseInformation>>(pros);

            foreach (var item in proVM)
            {
                if (item.Imagethumbnail != null)
                {
                    item.Imagethumbnail = await firebaseStorage.GetImageStreamAsync(item.Imagethumbnail);
                }
            }
            return View(proVM);
        }
        public IActionResult About()
        {

            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

    }
}
