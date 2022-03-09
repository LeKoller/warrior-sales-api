using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarriorSalesAPI.Data;
using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamsController : Controller
    {
        private readonly WarriorSalesAPIContext _context;

        public TeamsController(WarriorSalesAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Team>>> List()
        {
            return Ok(await _context.Teams.ToListAsync());
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        [HttpPost]
        public async Task<ActionResult<Team>> Create(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Created("Team created.", await _context.Teams.FindAsync(team.Id));
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
