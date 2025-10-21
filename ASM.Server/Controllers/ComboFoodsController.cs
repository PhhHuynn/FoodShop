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
    public class ComboFoodsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComboFoodsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ComboFoods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComboFood>>> GetComboFoods()
        {
            return await _context.ComboFoods.ToListAsync();
        }

        // GET: api/ComboFoods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComboFood>> GetComboFood(int id)
        {
            var comboFood = await _context.ComboFoods.FindAsync(id);

            if (comboFood == null)
            {
                return NotFound();
            }

            return comboFood;
        }

        // PUT: api/ComboFoods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComboFood(int id, ComboFood comboFood)
        {
            if (id != comboFood.Id)
            {
                return BadRequest();
            }

            _context.Entry(comboFood).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComboFoodExists(id))
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

        // POST: api/ComboFoods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ComboFood>> PostComboFood(ComboFood comboFood)
        {
            _context.ComboFoods.Add(comboFood);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComboFood", new { id = comboFood.Id }, comboFood);
        }

        // DELETE: api/ComboFoods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComboFood(int id)
        {
            var comboFood = await _context.ComboFoods.FindAsync(id);
            if (comboFood == null)
            {
                return NotFound();
            }

            _context.ComboFoods.Remove(comboFood);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComboFoodExists(int id)
        {
            return _context.ComboFoods.Any(e => e.Id == id);
        }
    }
}
