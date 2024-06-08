using Owls.Models;

namespace Owls.Repositories.ProductRepos
{
    public class ProductVariantsRepos : IProductVariant
    {
        private readonly OwlStoreContext _storeContext;

        public ProductVariantsRepos(OwlStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task<ProductVariant> GetVariant(string sku) => await _storeContext.ProductVariants.FindAsync(sku);
         
    }
}
