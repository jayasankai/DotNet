namespace ShopAppAPI.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAppAPI.Data;
using ShopAppAPI.Models;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;


[ApiVersion(1.0)]
[Route("api/v{v:apiVersion}/products")]
[ApiController]
public class ProductsV1Controller : ControllerBase
{
    private readonly ShopContext _context;

    public ProductsV1Controller(ShopContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    [MapToApiVersion( "1.0" )]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
    {
        IQueryable<Product> products = _context.Products;

        if (queryParameters.MinPrice != null)
        {
            products = products.Where(product => product.Price >= queryParameters.MinPrice.Value);
        }

        if (queryParameters.MaxPrice != null)
        {
            products = products.Where(product => product.Price <= queryParameters.MaxPrice.Value);
        }

        if (!String.IsNullOrEmpty(queryParameters.Sku))
        {
            products = products.Where(product => product.Sku == queryParameters.Sku);
        }

        if (!String.IsNullOrEmpty(queryParameters.Name))
        {
            products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
        }

        if (!String.IsNullOrEmpty(queryParameters.SortBy))
        {
            if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
            {
                products = products.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
            }
        }

        products = products
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        return Ok(await products.ToArrayAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(p => p.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return product;
    }

    [HttpPost]
    [Route("delete")]
    public async Task<ActionResult> DeleteProducts([FromQuery] int[] ids)
    {
        var products = new List<Product>();
        foreach (var id in ids)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            products.Add(product);
        }

        _context.Products.RemoveRange(products);
        await _context.SaveChangesAsync();

        return Ok(products);
    }
}

