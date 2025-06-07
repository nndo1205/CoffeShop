using CoffeeShop.Models;

namespace coffeeshop.Models.interfaces
{
    public interface IShoppingCartRepository
    {
        // Thêm sản phẩm vào giỏ hàng
        void AddToCart(Product product);
        // Xóa sản phẩm khỏi giỏ hàng, trả về số lượng còn lại của sản phẩm đó
        int RemoveFromCart(Product product);
        // Lấy tất cả các mặt hàng trong giỏ hàng
        List<ShoppingCartItem> GetAllShoppingCartItems();
        // Xóa tất cả các mặt hàng trong giỏ hàng
        void ClearCart();
        // Lấy tổng giá trị của giỏ hàng
        decimal GetShoppingCartTotal();

        // Thuộc tính để lưu trữ các mục trong giỏ hàng cho session hiện tại
        List<ShoppingCartItem>? ShoppingCartItems { get; set; }
        // Thuộc tính để lưu ID giỏ hàng cho session
        string? ShoppingCartId { get; set; }
    }
}
