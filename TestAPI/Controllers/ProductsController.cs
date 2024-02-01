using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Data;
using TestAPI.Models;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context) {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet("/api/test")]
        public string getProductsTest () {
            return "Success";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts() {
            var products = _context.Products.ToArray();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id) {
            var product = _context.Products.Find(id);

            if (product == null) {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
