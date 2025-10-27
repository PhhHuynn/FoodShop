using ASM.Server.Data;
using ASM.Server.DTOs;
using ASM.Server.Helpers;
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
    public class FoodsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            return await _context.Foods.ToListAsync();
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // PUT: api/Foods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> PutFood(int id, [FromBody] Food food)
        {
            if (id != food.Id)
            {
                return BadRequest();
            }

            var foodToUpdate = await _context.Foods.FindAsync(id);
            if (foodToUpdate == null)
            {
                return NotFound();
			}

            foodToUpdate.Name = food.Name;
            foodToUpdate.Price = food.Price;
            foodToUpdate.Description = food.Description;
            foodToUpdate.ImageUrl = food.ImageUrl;

			try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
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

        // POST: api/Foods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<Food>> PostFood([FromBody] Food newFood)
        {
			_context.Foods.Add(newFood);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFood", new { id = newFood.Id }, newFood);
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }

		[HttpPost("upload")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			var imagePath = await FileHelper.SaveFileAsync(file, "uploads/foods");
			return Ok(new { imageUrl = imagePath });
		}

		private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
}
