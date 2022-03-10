using Microsoft.AspNetCore.Mvc;
using WarriorSalesAPI.Data;
using WarriorSalesAPI.Services;
using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly WarriorSalesAPIContext _context;

        public HomeController(WarriorSalesAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            if (!_context.Products.Any() || !_context.Teams.Any())
            {
                return NoContent();
            }
            
            return Ok();
        }

        [HttpPost("seed")]
        public async Task<ActionResult> SeedDatabase()
        {
            if (!_context.Products.Any())
            {
                List<Product> productsSeed = ProductsService.CreateSeedList();
                _context.AddRange(productsSeed);
                await _context.SaveChangesAsync();
            }

            if (!_context.Teams.Any())
            {
                List<Team> teamsSeed = TeamsService.CreateSeedList();
                _context.AddRange(teamsSeed);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("auth")]
        public IActionResult SimulateAuthentication()
        {
            var token = TokenService.GenerateToken();

            return Ok(token);
        }
    }
}
