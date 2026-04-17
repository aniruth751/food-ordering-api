using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_context.Products.ToList());
        }

        // ADD
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }

        // UPDATE PRODUCT (NEW EDIT FEATURE)
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.ImageUrl = updatedProduct.ImageUrl;

            _context.SaveChanges();

            return Ok(product);
        }
    }
}