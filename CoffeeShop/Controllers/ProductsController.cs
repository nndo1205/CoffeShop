using coffee_shop.Models.interfaces;
using coffeeshop.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace coffeeshop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Shop()
        {
            var allProducts = _productRepository.GetAllProducts();
            return View(allProducts);
        }
        public ActionResult Detail(int id)
        {
            var product = _productRepository.GetProductDetail(id);
            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }
    }
}