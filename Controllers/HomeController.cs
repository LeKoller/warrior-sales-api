using Microsoft.AspNetCore.Mvc;
using WarriorSalesAPI.Data;
using WarriorSalesAPI.Services;

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
        public IActionResult Index()
        {
            // here we'll seed.
            return View();
        }

        [HttpPost("auth")]
        public IActionResult SimulateAuthentication()
        {
            var token = TokenService.GenerateToken();

            return Ok(token);
        }
    }
}
