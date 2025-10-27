using ASM.Server.Data;
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
    public class ComboesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComboesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Comboes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Combo>>> GetCombos()
        {
            return await _context.Combos.ToListAsync();
        }

        // GET: api/Comboes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Combo>> GetCombo(int id)
        {
			var combo = await _context.Combos
	        .Include(c => c.ComboFoods)
	        .ThenInclude(cf => cf.Food)
	        .FirstOrDefaultAsync(c => c.Id == id);


			if (combo == null)
            {
                return NotFound();
            }

            return combo;
        }

		// PUT: api/Comboes/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> PutCombo(int id, Combo combo)
		{
			if (id != combo.Id)
				return BadRequest();

			var existingCombo = await _context.Combos
				.Include(c => c.ComboFoods)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (existingCombo == null)
				return NotFound();

			existingCombo.Name = combo.Name;
			existingCombo.Description = combo.Description;
			existingCombo.Price = combo.Price;
			existingCombo.ImageUrl = combo.ImageUrl;
			existingCombo.IsAvailable = combo.IsAvailable;

			_context.ComboFoods.RemoveRange(existingCombo.ComboFoods);

			if (combo.ComboFoods != null)
			{
				foreach (var cf in combo.ComboFoods)
				{
					var foodExists = await _context.Foods.AnyAsync(f => f.Id == cf.FoodId);
					if (foodExists)
					{
						_context.ComboFoods.Add(new ComboFood
						{
							ComboId = existingCombo.Id,
							FoodId = cf.FoodId,
							Quantity = cf.Quantity
						});
					}
				}
			}

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
			}

			return NoContent();
		}


		[Authorize(Roles = "Admin")]
		[HttpPost("upload")]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			var imagePath = await FileHelper.SaveFileAsync(file, "uploads/comboes");
			return Ok(new { imageUrl = imagePath });
		}

		// POST: api/Comboes
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<Combo>> PostCombo(Combo combo)
        {
            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCombo", new { id = combo.Id }, combo);
        }

        // DELETE: api/Comboes/5
        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteCombo(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null)
            {
                return NotFound();
            }

            _context.Combos.Remove(combo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComboExists(int id)
        {
            return _context.Combos.Any(e => e.Id == id);
        }
    }
}
