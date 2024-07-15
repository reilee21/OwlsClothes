using Microsoft.EntityFrameworkCore;
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

        public async Task<ProductVariant> GetVariant(string sku)
        {
            var variant = await _storeContext.ProductVariants.FirstOrDefaultAsync(v => v.Sku.Equals(sku));
            if (variant == null)
                return null;
            var promotions = await _storeContext.Promotions.Where(p => p.Products.Any(p => p.Sku == variant.Sku)).ToListAsync();
            ProductVariant rs = new ProductVariant()
            {
                Sku = variant.Sku,
                Cost = variant.Cost,
                SalePrice = variant.SalePrice,
                ColorId = variant.ColorId,
                ProductId = variant.ProductId,
                Size = variant.Size,
                Quantity = variant.Quantity,
                Promotions = promotions
            };

            return rs;
        }
    }
}
