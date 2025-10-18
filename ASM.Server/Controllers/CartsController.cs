using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM.Server.Data;
using ASM.Server.Models;

namespace ASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartsController(AppDbContext context)
        {
            _context = context;
        }

		// GET: api/Carts/active/2
		[HttpGet("active/{userId}")]
        public async Task<ActionResult<Cart>> GetActiveCart(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Food)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == CartStatus.Active);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Status = CartStatus.Active,
                    CartDetails = new List<CartDetail>()
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
			}
			return cart;
		}

        // POST: api/Carts/1/add
        [HttpPost("{cartId}/add")]
        public async Task<ActionResult> AddToCart(int cartId, [FromBody] CartDetail cartDetail)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null ||  cart.Status != CartStatus.Active)
            {
                return BadRequest("Cart not found or not active");
            }

            var existing = cart.CartDetails.FirstOrDefault(d => d.Id == cartDetail.Id);
            if (existing == null)
            {
                cart.CartDetails.Add(new CartDetail
                {
                    FoodId = cartDetail.FoodId,
                    CartId = cart.Id,
                    Quantity = cartDetail.Quantity,
                });
            }
            else
            {
                existing.Quantity += 1;
            }

            await _context.SaveChangesAsync();
            return Ok(cart);
		}

        // DELETE: api/Carts/1/remove/2
        [HttpDelete("{cartId}/remove/{foodId}")]
        public async Task<IActionResult> RemoveFromCart(int cartId, int foodId)
        {
            var item = await _context.CartDetails
                .FirstOrDefaultAsync(d => d.CartId == cartId && d.FoodId == foodId);

            if (item == null) {
                return NotFound();
            }

            _context.CartDetails.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
		}

		[HttpPost("{cartId}/checkout")]
        public async Task<IActionResult> Checkout(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null) return NotFound();

            if (!cart.CartDetails.Any()) return BadRequest("Cart is empty");

            cart.Status = CartStatus.CheckedOut;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Checkout successful"});
        }

		[HttpPut("{cartId}/items/{foodId}")]
		public async Task<IActionResult> UpdateCartItemQuantity(int cartId, int foodId, [FromBody] int quantity)
		{
			var item = await _context.CartDetails
				.FirstOrDefaultAsync(d => d.CartId == cartId && d.FoodId == foodId);

			if (item == null)
				return NotFound("Item not found in the cart.");

			if (quantity <= 0)
				return BadRequest("Quantity must be greater than 0.");

			item.Quantity = quantity;

			await _context.SaveChangesAsync();

			return Ok(new { message = "Quantity updated successfully", item });
		}


	}
}
