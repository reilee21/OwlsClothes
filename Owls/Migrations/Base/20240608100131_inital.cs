using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Owls.Migrations.Base
{
    public partial class inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ColorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ColorId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTotal = table.Column<double>(type: "float", nullable: true),
                    ShippingFee = table.Column<double>(type: "float", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "ShippingFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fee = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagethumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ImgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ImgId);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<double>(type: "float", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorId = table.Column<int>(type: "int", nullable: true),
                    SalePrice = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Sku);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "ColorId");
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_ProductVariants_Sku",
                        column: x => x.Sku,
                        principalTable: "ProductVariants",
                        principalColumn: "Sku",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ShippingFees",
                columns: new[] { "Id", "City", "Fee" },
                values: new object[,]
                {
                    { 1, "Thành phố Hà Nội", 20000 },
                    { 2, "Tỉnh Hà Giang", 20000 },
                    { 3, "Tỉnh Cao Bằng", 20000 },
                    { 4, "Tỉnh Bắc Kạn", 20000 },
                    { 5, "Tỉnh Tuyên Quang", 20000 },
                    { 6, "Tỉnh Lào Cai", 20000 },
                    { 7, "Tỉnh Điện Biên", 20000 },
                    { 8, "Tỉnh Lai Châu", 20000 },
                    { 9, "Tỉnh Sơn La", 20000 },
                    { 10, "Tỉnh Yên Bái", 20000 },
                    { 11, "Tỉnh Hoà Bình", 20000 },
                    { 12, "Tỉnh Thái Nguyên", 20000 },
                    { 13, "Tỉnh Lạng Sơn", 20000 },
                    { 14, "Tỉnh Quảng Ninh", 20000 },
                    { 15, "Tỉnh Bắc Giang", 20000 },
                    { 16, "Tỉnh Phú Thọ", 20000 },
                    { 17, "Tỉnh Vĩnh Phúc", 20000 },
                    { 18, "Tỉnh Bắc Ninh", 20000 },
                    { 19, "Tỉnh Hải Dương", 20000 },
                    { 20, "Thành phố Hải Phòng", 20000 },
                    { 21, "Tỉnh Hưng Yên", 20000 },
                    { 22, "Tỉnh Thái Bình", 20000 },
                    { 23, "Tỉnh Hà Nam", 20000 },
                    { 24, "Tỉnh Nam Định", 20000 },
                    { 25, "Tỉnh Ninh Bình", 20000 },
                    { 26, "Tỉnh Thanh Hóa", 20000 },
                    { 27, "Tỉnh Nghệ An", 20000 },
                    { 28, "Tỉnh Hà Tĩnh", 20000 },
                    { 29, "Tỉnh Quảng Bình", 20000 },
                    { 30, "Tỉnh Quảng Trị", 20000 },
                    { 31, "Tỉnh Thừa Thiên Huế", 20000 },
                    { 32, "Thành phố Đà Nẵng", 20000 },
                    { 33, "Tỉnh Quảng Nam", 20000 },
                    { 34, "Tỉnh Quảng Ngãi", 20000 },
                    { 35, "Tỉnh Bình Định", 20000 },
                    { 36, "Tỉnh Phú Yên", 20000 },
                    { 37, "Tỉnh Khánh Hòa", 20000 },
                    { 38, "Tỉnh Ninh Thuận", 20000 },
                    { 39, "Tỉnh Bình Thuận", 20000 },
                    { 40, "Tỉnh Kon Tum", 20000 },
                    { 41, "Tỉnh Gia Lai", 20000 },
                    { 42, "Tỉnh Đắk Lắk", 20000 }
                });

            migrationBuilder.InsertData(
                table: "ShippingFees",
                columns: new[] { "Id", "City", "Fee" },
                values: new object[,]
                {
                    { 43, "Tỉnh Đắk Nông", 20000 },
                    { 44, "Tỉnh Lâm Đồng", 20000 },
                    { 45, "Tỉnh Bình Phước", 20000 },
                    { 46, "Tỉnh Tây Ninh", 20000 },
                    { 47, "Tỉnh Bình Dương", 20000 },
                    { 48, "Tỉnh Đồng Nai", 20000 },
                    { 49, "Tỉnh Bà Rịa - Vũng Tàu", 20000 },
                    { 50, "Thành phố Hồ Chí Minh", 20000 },
                    { 51, "Tỉnh Long An", 20000 },
                    { 52, "Tỉnh Tiền Giang", 20000 },
                    { 53, "Tỉnh Bến Tre", 20000 },
                    { 54, "Tỉnh Trà Vinh", 20000 },
                    { 55, "Tỉnh Vĩnh Long", 20000 },
                    { 56, "Tỉnh Đồng Tháp", 20000 },
                    { 57, "Tỉnh An Giang", 20000 },
                    { 58, "Tỉnh Kiên Giang", 20000 },
                    { 59, "Thành phố Cần Thơ", 20000 },
                    { 60, "Tỉnh Hậu Giang", 20000 },
                    { 61, "Tỉnh Sóc Trăng", 20000 },
                    { 62, "Tỉnh Bạc Liêu", 20000 },
                    { 63, "Tỉnh Cà Mau", 20000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_Sku",
                table: "OrderDetails",
                column: "Sku");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ColorId",
                table: "ProductVariants",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ShippingFees");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
