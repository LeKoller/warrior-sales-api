#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarriorSalesAPI.Data;
using WarriorSalesAPI.DTOs;
using WarriorSalesAPI.Models;
using WarriorSalesAPI.Services;

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
        [AllowAnonymous]
        public async Task<ActionResult<OrdersPaginationDTO>> List(
            [FromQuery] int page = 1,
            [FromQuery] int results = 10)
        {
            int pageCount = (int)Math.Ceiling(_context.Orders.Count() / (float)results);

            var orders = await _context.Orders
                .OrderByDescending(o => o.Creation)
                .Skip((page - 1) * results)
                .Take(results)
                .Include(o => o.Team)
                .ToListAsync();

            List<OrderListDTO> ordersDTO = OrdersService.GenerateListOfOrderListDTO(orders);
            OrdersPaginationDTO responseContent = new()
            {
                CurrentPage = page,
                Pages = pageCount,
                Orders = ordersDTO
            };

            return Ok(responseContent);
        }

        [HttpGet("{id}")]
        [Authorize]
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> Create(AddOrderDTO addOrderDTO)
        {
            List<Team> teams = await _context.Teams.ToListAsync();
            var generated = OrdersService.GenerateOrder(teams, addOrderDTO);
            
            if (generated.Error) { return BadRequest(generated.Message); }

            Order order = generated.Payload;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var createdOrder = await _context.Orders.FindAsync(order.Id);

            if (createdOrder == null) { return BadRequest("Could not create order."); }

            foreach (CartItem cartItem in addOrderDTO.Cart)
            {
                var product = await _context.Products.FindAsync(cartItem.Id);
                var validate = OrdersService.ValidateSaleItemWithProduct(cartItem, product);
                int quantity = cartItem.Quantity;

                if (validate.Error) { return BadRequest(validate.Message); }

                SaleItem saleItem = OrdersService.GenerateSaleItem(product, createdOrder, quantity);
                
                product.Stock -= quantity;
                _context.SaleItems.Add(saleItem);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();

            return Created("Order created.", createdOrder);
        }

        [HttpPatch("{id}")]
        [Authorize]
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

        [HttpPut("{id}")]
        [Authorize]
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
        [Authorize]
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
