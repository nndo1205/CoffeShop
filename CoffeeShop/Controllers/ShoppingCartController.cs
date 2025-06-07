using coffeeshop.Models.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Cần thiết để truy cập HttpContext.Session.SetInt32
using coffee_shop.Models.interfaces;

namespace coffeeshop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var items = _shoppingCartRepository.GetAllShoppingCartItems();
            _shoppingCartRepository.ShoppingCartItems = items; // Cập nhật thuộc tính trong repository

            ViewBag.TotalCart = _shoppingCartRepository.GetShoppingCartTotal();

            return View(items);
        }

        public RedirectToActionResult AddToShoppingCart(int pId)
        {
            var product = _productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);

            if (product != null)
            {
                _shoppingCartRepository.AddToCart(product);
                // Cập nhật số lượng mặt hàng trong giỏ hàng vào Session sau khi thêm
                int cartCount = _shoppingCartRepository.GetAllShoppingCartItems().Count();
                HttpContext.Session.SetInt32("CartCount", cartCount);
            }
            return RedirectToAction("Index"); // Chuyển hướng về trang giỏ hàng
        }

        // Phương thức này sẽ xử lý việc xóa sản phẩm khỏi giỏ hàng
        public RedirectToActionResult RemoveFromShoppingCart(int pId)
        {
            // Tìm sản phẩm cần xóa dựa trên ID
            var product = _productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);

            if (product != null)
            {
                // Gọi phương thức RemoveFromCart của repository để xóa sản phẩm
                _shoppingCartRepository.RemoveFromCart(product);
                // Cập nhật số lượng mặt hàng trong giỏ hàng vào Session sau khi xóa
                int cartCount = _shoppingCartRepository.GetAllShoppingCartItems().Count();
                HttpContext.Session.SetInt32("CartCount", cartCount);
            }
            return RedirectToAction("Index"); // Chuyển hướng về trang giỏ hàng để hiển thị giỏ hàng đã cập nhật
        }
    }
}
