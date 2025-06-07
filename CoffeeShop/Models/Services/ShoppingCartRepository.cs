using coffeeshop.Data;
using coffeeshop.Models.interfaces;
using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore; // Required for .Include() method

namespace coffeeshop.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CoffeeshopDbContext _dbContext; // Renamed for consistency

        // This property will hold the current shopping cart items for the session
        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }
        // This property will hold the unique ID for the current shopping cart session
        public string? ShoppingCartId { get; set; } // Initialized from GetCart method

        public ShoppingCartRepository(CoffeeshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Static method to get the shopping cart for the current session
        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            // Get HttpContextAccessor to access session
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            // Get DbContext instance
            CoffeeshopDbContext context = services.GetService<CoffeeshopDbContext>() ??
                throw new Exception("Error initializing coffeeshopdbcontext");

            // Get or create a unique cart ID for the session
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);

            // Return a new ShoppingCartRepository instance with the current cart ID
            return new ShoppingCartRepository(context) { ShoppingCartId = cartId };
        }

        // Adds a product to the shopping cart
        public void AddToCart(Product product)
        {
            var shoppingCartItem = _dbContext.ShoppingCartItems.FirstOrDefault(s =>
                s.Product!.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                // If product not in cart, add new item
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Qty = 1
                };
                _dbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                // If product already in cart, increase quantity
                shoppingCartItem.Qty++;
            }
            _dbContext.SaveChanges();
        }

        // Removes a product from the shopping cart
        public int RemoveFromCart(Product product)
        {
            var shoppingCartItem = _dbContext.ShoppingCartItems.FirstOrDefault(s =>
                s.Product!.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

            int quantity = 0; // Quantity remaining after removal

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Qty > 1)
                {
                    // If quantity > 1, decrease quantity
                    shoppingCartItem.Qty--;
                    quantity = shoppingCartItem.Qty;
                }
                else
                {
                    // If quantity is 1, remove the item completely
                    _dbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            _dbContext.SaveChanges();
            return quantity;
        }

        // Gets all items in the shopping cart for the current session
        public List<ShoppingCartItem> GetAllShoppingCartItems()
        {
            // If ShoppingCartItems is null, fetch from DB, otherwise return existing list
            return ShoppingCartItems ??= _dbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId)
                .Include(p => p.Product) // Eagerly load product details
                .ToList();
        }

        // Clears all items from the shopping cart for the current session
        public void ClearCart()
        {
            var cartItems = _dbContext.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId);
            _dbContext.ShoppingCartItems.RemoveRange(cartItems);
            _dbContext.SaveChanges();
        }

        // Calculates the total cost of all items in the shopping cart
        public decimal GetShoppingCartTotal()
        {
            var totalCost = _dbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId)
                .Select(s => s.Product!.Price * s.Qty) // Calculate price * quantity for each item
                .Sum(); // Sum up all costs

            return totalCost;
        }
    }
}