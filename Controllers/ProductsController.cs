using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarriorSalesAPI.Data;
using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly WarriorSalesAPIContext _context;

        public ProductsController(WarriorSalesAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> List()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> Retrieve(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Created("Product created.", await _context.Products.FindAsync(product.Id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(Product request, int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;  
            product.Category = request.Category;
            product.Stock = request.Stock;

            await _context.SaveChangesAsync();

            return Ok(await _context.Products.FindAsync(product.Id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }
}
