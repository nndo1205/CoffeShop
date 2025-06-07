using CoffeeShop.Models;
using System.ComponentModel.DataAnnotations;

namespace coffeeshop.Models
{
    public class ShoppingCartItem
    {
        [Key] // Sets Id as the primary key
        public int Id { get; set; }

        public Product? Product { get; set; } // The product being added to the cart
        public int Qty { get; set; } // Quantity of the product
        public string? ShoppingCartId { get; set; } // Unique ID for the shopping cart session
    }
}
