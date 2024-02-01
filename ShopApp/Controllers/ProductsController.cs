using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAppAPI.Data;
using ShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ShopAppAPI.Controllers
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
            var products = await _context.Products.ToArrayAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id) {
            var product = await _context.Products.FindAsync(id);

            if (product == null) {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product) {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product) {
            if (id != product.Id) {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!_context.Products.Any(p => p.Id == id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id) {
            var product = await _context.Products.FindAsync(id);

            if (product == null) {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteProducts([FromQuery]int[] ids) {
            var products = new List<Product>();
            foreach(var id in ids) {
                var product = await _context.Products.FindAsync(id);

                if (product == null) {
                    return NotFound();
                }

                products.Add(product);
            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();

            return Ok(products);
        }
    }
}