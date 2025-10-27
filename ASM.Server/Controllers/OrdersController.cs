using ASM.Server.Data;
using ASM.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

		// GET: api/Orders/user/2
		[HttpGet("user/{userId}")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId(string userId)
		{
			var orders = await _context.Orders
				.Include(o => o.OrderDetails)
					.ThenInclude(od => od.Food)
				.Include(o => o.OrderDetails)
					.ThenInclude(od => od.Combo)
				.Where(o => o.UserId == userId)
				.OrderByDescending(o => o.CreatedAt)
				.ToListAsync();

			if (!orders.Any())
			{
				return NotFound("Người dùng này chưa có đơn hàng nào.");
			}

			return Ok(orders);
		}


		// GET: api/Orders/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

		// PUT: api/Orders/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] int status)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order == null)
			{
				return NotFound();
			}

			order.Status = (OrderStatus)status;

			_context.Entry(order).Property(o => o.Status).IsModified = true;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{	
				if (!OrderExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Orders
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
