// Đảm bảo bạn có đủ các dòng using này ở đầu tệp
using coffee_shop.Models.interfaces;
using coffeeshop.Models.Services;
using coffeeshop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using coffeeshop.Models.interfaces;

var builder = WebApplication.CreateBuilder(args);

// --- BƯỚC 1: LẤY CHUỖI KẾT NỐI ---
var connectionString = builder.Configuration.GetConnectionString("CoffeeShopDbContextConnection") ?? throw new InvalidOperationException("Connection string 'CoffeeShopDbContextConnection' not found.");


// --- BƯỚC 2: ĐĂNG KÝ CÁC DỊCH VỤ VỚI CONTAINER ---

builder.Services.AddDbContext<CoffeeshopDbContext>(options =>
    options.UseSqlServer(connectionString)); 

// Đăng ký dịch vụ cho Controller và View
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IShoppingCartRepository>(sp => ShoppingCartRepository.GetCart(sp));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn
    options.Cookie.HttpOnly = true; // Cookie chỉ truy cập qua HTTP
    options.Cookie.IsEssential = true; // Session cookie là cần thiết
});
builder.Services.AddHttpContextAccessor();

// --- BƯỚC 3: CẤU HÌNH HTTP REQUEST PIPELINE ---
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeshopDbContext>();
    dbContext.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();