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
        
        [HttpPost]
        public async Task<ActionResult<Team>> Create(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Created("Team created.", await _context.Teams.FindAsync(team.Id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (TeamExists(id))
            {
                var team = await _context.Teams.FindAsync(id);

                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();

                return NoContent(); ;
            }

            return NotFound("Order not found.");
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
