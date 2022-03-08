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
        // private static List<Product> products = new()
        // {
        //     new Product
        //         {
        //             Id = 1,
        //             Name = "Beer",
        //             Description = "Stout beer.",
        //             Price = 16.9f,
        //             Stock = 99,
        //             Type = "Alcoholic bavarege."
        //         }
        // };

        private readonly WarriorSalesAPIContext _context;

        public ProductsController(WarriorSalesAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetList()
        {
            return Ok(await _context.Products.ToListAsync()); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Created("", await _context.Products.FindAsync(product.Id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(Product request, int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;  
            product.Type = request.Type;
            product.Stock = request.Stock;

            await _context.SaveChangesAsync();

            return Ok(await _context.Products.FindAsync(product.Id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
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
