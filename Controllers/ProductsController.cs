using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarriorSalesAPI.Data;
using WarriorSalesAPI.DTOs;
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
        public async Task<ActionResult<List<Product>>> List(
            [FromQuery] int page = 1,
            [FromQuery] int results = 20)
        {
            int productsCount = _context.Products.Count();
            int pageCount = (int)Math.Ceiling(productsCount / (float)results);

            var products = await _context.Products
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * results)
                .Take(results)
                .ToListAsync();

            ProductsPaginationDTO responseContent = new()
            {
                CurrentPage = page,
                Pages = pageCount,
                Products = products,
                Total = productsCount,
            };

            return Ok(responseContent);
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
            var isThereHomonymousProduct = await _context.Products.AnyAsync(p => p.Name == product.Name);

            if (isThereHomonymousProduct == true)
            {
                return BadRequest("There is already a product registerer under the name " + product.Name + ".");
            }

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
