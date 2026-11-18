using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using StorageApi.DTOs;
using StorageApi.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly StorageApiContext _context;

    public ProductsController(StorageApiContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct([FromQuery] string? category, [FromQuery] string? name)
    {
        var query = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(category))
        {
            var categoryFilter = category.Trim();
            query = query.Where(product => product.Category == categoryFilter);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            var nameFilter = name.Trim();
            query = query.Where(product => product.Name.Contains(nameFilter));
        }

        return await query
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Count = product.Count
            })
            .ToListAsync();
    }

    // GET: api/products/stats
    [HttpGet("stats")]
    public async Task<ActionResult<ProductStatsDto>> GetStats()
    {
        var stats = await _context.Products
            .AsNoTracking()
            .GroupBy(product => 1)
            .Select(products => new ProductStatsDto
            {
                TotalProducts = products.Count(),
                TotalInventoryValue = products.Sum(product => (long)product.Price * product.Count),
                AveragePrice = products.Average(product => (decimal)product.Price)
            })
            .SingleOrDefaultAsync();

        return Ok(stats ?? new ProductStatsDto());
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return ToDto(product);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, CreateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Category = dto.Category;
        product.Shelf = dto.Shelf;
        product.Count = dto.Count;
        product.Description = dto.Description;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Products.AnyAsync(product => product.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category,
            Shelf = dto.Shelf,
            Count = dto.Count,
            Description = dto.Description
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, ToDto(product));
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static ProductDto ToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        Count = product.Count
    };
}