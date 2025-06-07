using coffeeshop.Models;
using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace coffeeshop.Data
{
    public class CoffeeshopDbContext : DbContext
    {
        public CoffeeshopDbContext(DbContextOptions<CoffeeshopDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        // Thêm DbSet cho ShoppingCartItem
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        // Phương thức này được thêm vào để khắc phục một sự cố nghiêm trọng từ tài liệu
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SỬA LỖI: Cấu hình thuộc tính 'Price' để ánh xạ tới một kiểu decimal cụ thể trong SQL Server
            // Điều này ngăn chặn cảnh báo và khả năng mất dữ liệu được đề cập trong tài liệu
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");

            // Đoạn mã này thêm dữ liệu ban đầu (seed data) vào cơ sở dữ liệu
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "America", Price = 25, Detail = "Name product", ImageUrl = "https://insanelygoodrecipes.com/wp-content/uploads/2020/07/Cup-Of-Creamy-Coffee-1024x536.webp" },
                new Product { Id = 2, Name = "Vietnam", Price = 20, Detail = "Vietnamese product", ImageUrl = "https://insanelygoodrecipes.com/wp-content/uploads/2020/07/Cup-Of-Creamy-Coffee-1024x536.webp" },
                new Product { Id = 3, Name = "United Kingdom", Price = 15, Detail = "UK product", ImageUrl = "https://insanelygoodrecipes.com/wp-content/uploads/2020/07/Cup-Of-Creamy-Coffee-1024x536.webp" }
                // Dữ liệu mẫu từ tài liệu 
            );
        }
    }
}