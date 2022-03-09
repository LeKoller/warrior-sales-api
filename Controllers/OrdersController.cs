#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarriorSalesAPI.Data;
using WarriorSalesAPI.DTOs;
using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly WarriorSalesAPIContext _context;

        public OrdersController(WarriorSalesAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> List()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.Team)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Retrieve(int id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Items)
                .Include(o => o.Team)
                .FirstAsync();

            if (order == null)
            {
                return BadRequest("Order not found.");
            }

            return Ok(order);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<Order>> Create(AddOrderDTO addOrderDTO)
        {
            List<Team> teams = await _context.Teams.ToListAsync();

            if (teams.Count == 0)
            {
                return BadRequest("No team avaiable to execute the order.");
            }

            int randomIndex = new Random().Next(teams.Count);
            Team randomTeam = teams[randomIndex];

            var order = new Order { Address = addOrderDTO.Address, Team = randomTeam };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            var createdOrder = await _context.Orders.FindAsync(order.Id);

            if (createdOrder == null) { return BadRequest("Could not create order."); }

            foreach (CartItem cartItem in addOrderDTO.Cart)
            {
                var product = await _context.Products.FindAsync(cartItem.Id);

                if (product == null)
                {
                    return BadRequest($"Product named {cartItem.Name} was not found.");
                }

                if (cartItem.Quantity > product.Stock)
                {
                    return Forbid(
                        $"Product named {cartItem.Name} has an insuficient stock of {product.Stock} items."
                    );
                }

                product.Stock -= cartItem.Quantity;

                SaleItem saleItem = new()
                { 
                    Name = product.Name, 
                    Description = product.Description,
                    Order = createdOrder,
                    Price = product.Price,
                    Quantity = cartItem.Quantity,
                    Product = product,
                };

                _context.SaleItems.Add(saleItem);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
                
            return Created("Order created.", order);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Order>> SetDelivered(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Delivery = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(await _context.Orders.FindAsync(id));
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateOrderDTO updateOrderDTO)
        {
            if (id != updateOrderDTO.Id)
            {
                return BadRequest("The id param do not match with the object id.");
            }

            if (ModelState.IsValid)
            {
                var order = await _context.Orders.FindAsync(id);
                
                if (!OrderExists(order.Id))
                {
                    return NotFound("Order not found.");
                }

                order.Address = updateOrderDTO.Address;
                await _context.SaveChangesAsync();
            }
            return Ok(await _context.Orders.FindAsync(id));
        }

        
        [HttpDelete("{id}")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (OrderExists(id))
            {
                var order = await _context.Orders.FindAsync(id);

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return NoContent(); ;
            }

            return NotFound("Order not found.");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
