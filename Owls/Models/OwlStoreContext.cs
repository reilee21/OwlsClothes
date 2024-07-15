using Microsoft.EntityFrameworkCore;

namespace Owls.Models
{
    public partial class OwlStoreContext : DbContext
    {
        public OwlStoreContext() { }

        public OwlStoreContext(DbContextOptions<OwlStoreContext> options) : base(options) { }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
        public virtual DbSet<ProductVariant> ProductVariants { get; set; } = null!;
        public virtual DbSet<ShippingFee> ShippingFees { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;
        public virtual DbSet<Promotion> Promotions { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductVariant>().HasMany(e => e.OrderDetails)
                .WithOne(e => e.ProductVariant)
                .HasForeignKey(e => e.Sku)
                .IsRequired();
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);
                entity.HasOne(e => e.Order)
                                             .WithMany(o => o.OrderDetails)
                                             .HasForeignKey(e => e.OrderId)
                                             .IsRequired();
            });


            modelBuilder.Entity<ShippingFee>().HasData(
                    new ShippingFee { Id = 1, City = "Thành phố Hà Nội", Fee = 20000 },
                    new ShippingFee { Id = 2, City = "Tỉnh Hà Giang", Fee = 20000 },
                    new ShippingFee { Id = 3, City = "Tỉnh Cao Bằng", Fee = 20000 },
                    new ShippingFee { Id = 4, City = "Tỉnh Bắc Kạn", Fee = 20000 },
                    new ShippingFee { Id = 5, City = "Tỉnh Tuyên Quang", Fee = 20000 },
                    new ShippingFee { Id = 6, City = "Tỉnh Lào Cai", Fee = 20000 },
                    new ShippingFee { Id = 7, City = "Tỉnh Điện Biên", Fee = 20000 },
                    new ShippingFee { Id = 8, City = "Tỉnh Lai Châu", Fee = 20000 },
                    new ShippingFee { Id = 9, City = "Tỉnh Sơn La", Fee = 20000 },
                    new ShippingFee { Id = 10, City = "Tỉnh Yên Bái", Fee = 20000 },
                    new ShippingFee { Id = 11, City = "Tỉnh Hoà Bình", Fee = 20000 },
                    new ShippingFee { Id = 12, City = "Tỉnh Thái Nguyên", Fee = 20000 },
                    new ShippingFee { Id = 13, City = "Tỉnh Lạng Sơn", Fee = 20000 },
                    new ShippingFee { Id = 14, City = "Tỉnh Quảng Ninh", Fee = 20000 },
                    new ShippingFee { Id = 15, City = "Tỉnh Bắc Giang", Fee = 20000 },
                    new ShippingFee { Id = 16, City = "Tỉnh Phú Thọ", Fee = 20000 },
                    new ShippingFee { Id = 17, City = "Tỉnh Vĩnh Phúc", Fee = 20000 },
                    new ShippingFee { Id = 18, City = "Tỉnh Bắc Ninh", Fee = 20000 },
                    new ShippingFee { Id = 19, City = "Tỉnh Hải Dương", Fee = 20000 },
                    new ShippingFee { Id = 20, City = "Thành phố Hải Phòng", Fee = 20000 },
                    new ShippingFee { Id = 21, City = "Tỉnh Hưng Yên", Fee = 20000 },
                    new ShippingFee { Id = 22, City = "Tỉnh Thái Bình", Fee = 20000 },
                    new ShippingFee { Id = 23, City = "Tỉnh Hà Nam", Fee = 20000 },
                    new ShippingFee { Id = 24, City = "Tỉnh Nam Định", Fee = 20000 },
                    new ShippingFee { Id = 25, City = "Tỉnh Ninh Bình", Fee = 20000 },
                    new ShippingFee { Id = 26, City = "Tỉnh Thanh Hóa", Fee = 20000 },
                    new ShippingFee { Id = 27, City = "Tỉnh Nghệ An", Fee = 20000 },
                    new ShippingFee { Id = 28, City = "Tỉnh Hà Tĩnh", Fee = 20000 },
                    new ShippingFee { Id = 29, City = "Tỉnh Quảng Bình", Fee = 20000 },
                    new ShippingFee { Id = 30, City = "Tỉnh Quảng Trị", Fee = 20000 },
                    new ShippingFee { Id = 31, City = "Tỉnh Thừa Thiên Huế", Fee = 20000 },
                    new ShippingFee { Id = 32, City = "Thành phố Đà Nẵng", Fee = 20000 },
                    new ShippingFee { Id = 33, City = "Tỉnh Quảng Nam", Fee = 20000 },
                    new ShippingFee { Id = 34, City = "Tỉnh Quảng Ngãi", Fee = 20000 },
                    new ShippingFee { Id = 35, City = "Tỉnh Bình Định", Fee = 20000 },
                    new ShippingFee { Id = 36, City = "Tỉnh Phú Yên", Fee = 20000 },
                    new ShippingFee { Id = 37, City = "Tỉnh Khánh Hòa", Fee = 20000 },
                    new ShippingFee { Id = 38, City = "Tỉnh Ninh Thuận", Fee = 20000 },
                    new ShippingFee { Id = 39, City = "Tỉnh Bình Thuận", Fee = 20000 },
                    new ShippingFee { Id = 40, City = "Tỉnh Kon Tum", Fee = 20000 },
                    new ShippingFee { Id = 41, City = "Tỉnh Gia Lai", Fee = 20000 },
                    new ShippingFee { Id = 42, City = "Tỉnh Đắk Lắk", Fee = 20000 },
                    new ShippingFee { Id = 43, City = "Tỉnh Đắk Nông", Fee = 20000 },
                    new ShippingFee { Id = 44, City = "Tỉnh Lâm Đồng", Fee = 20000 },
                    new ShippingFee { Id = 45, City = "Tỉnh Bình Phước", Fee = 20000 },
                    new ShippingFee { Id = 46, City = "Tỉnh Tây Ninh", Fee = 20000 },
                    new ShippingFee { Id = 47, City = "Tỉnh Bình Dương", Fee = 20000 },
                    new ShippingFee { Id = 48, City = "Tỉnh Đồng Nai", Fee = 20000 },
                    new ShippingFee { Id = 49, City = "Tỉnh Bà Rịa - Vũng Tàu", Fee = 20000 },
                    new ShippingFee { Id = 50, City = "Thành phố Hồ Chí Minh", Fee = 20000 },
                    new ShippingFee { Id = 51, City = "Tỉnh Long An", Fee = 20000 },
                    new ShippingFee { Id = 52, City = "Tỉnh Tiền Giang", Fee = 20000 },
                    new ShippingFee { Id = 53, City = "Tỉnh Bến Tre", Fee = 20000 },
                    new ShippingFee { Id = 54, City = "Tỉnh Trà Vinh", Fee = 20000 },
                    new ShippingFee { Id = 55, City = "Tỉnh Vĩnh Long", Fee = 20000 },
                    new ShippingFee { Id = 56, City = "Tỉnh Đồng Tháp", Fee = 20000 },
                    new ShippingFee { Id = 57, City = "Tỉnh An Giang", Fee = 20000 },
                    new ShippingFee { Id = 58, City = "Tỉnh Kiên Giang", Fee = 20000 },
                    new ShippingFee { Id = 59, City = "Thành phố Cần Thơ", Fee = 20000 },
                    new ShippingFee { Id = 60, City = "Tỉnh Hậu Giang", Fee = 20000 },
                    new ShippingFee { Id = 61, City = "Tỉnh Sóc Trăng", Fee = 20000 },
                    new ShippingFee { Id = 62, City = "Tỉnh Bạc Liêu", Fee = 20000 },
                    new ShippingFee { Id = 63, City = "Tỉnh Cà Mau", Fee = 20000 }
            );
        }
    }
}
