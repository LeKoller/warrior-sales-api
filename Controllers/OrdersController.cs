﻿#nullable disable
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
        public async Task<ActionResult<List<Order>>> Index()
        {
            List<Product> products = new();
            var orders = await _context.Orders.ToListAsync();

            // foreach (var order in orders)
            // {
            //     var orderProducts = await _context.OrderProducts.Where(op => op.OrderId == order.Id).ToListAsync();
            // 
            //     foreach(var op in orderProducts)
            //     {
            //         var product = await _context.Products.FindAsync(op.ProductId);
            // 
            //         products.Add(product);
            //     }
            // 
            //     order.Products = products;
            // }

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
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
                BadRequest("No team avaiable.");
            }

            int randomIndex = new Random().Next(teams.Count);
            Team randomTeam = teams[randomIndex];

            var order = new Order { Address = addOrderDTO.Address, TeamId = randomTeam.Id };
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

        // [HttpPut("{id}")]
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        // 
        //     var order = await _context.Order.FindAsync(id);
        //     if (order == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(order);
        // }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Creation,Delivery,Address")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        // 
        //     var order = await _context.Order
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (order == null)
        //     {
        //         return NotFound();
        //     }
        // 
        //     return View(order);
        // }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}