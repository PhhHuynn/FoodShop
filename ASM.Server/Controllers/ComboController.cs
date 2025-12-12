using ASM.Server.Data;
using ASM.Server.Dtos;
using ASM.Server.Dtos.ComboDtos;
using ASM.Server.Helpers;
using ASM.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace ASM.Server.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;


		public ComboController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ================================================================
        // GET ACTIVE COMBOS
        // ================================================================
        /// <summary>
        /// Lấy danh sách tất cả combo (chỉ combo chưa bị xóa) và không bao gồm luôn comboFood.
        /// </summary>
        /// <returns>Danh sách combo.</returns>
        [HttpGet("active")]
		public async Task<ActionResult<IEnumerable<ComboResponDto>>> GetActive()
		{
			var result = await _context.Combos
				.Where(c => c.DeletedAt == null || c.IsAvailable)
				.Select(c => new ComboResponDto
				{
					Id = c.Id,
					Name = c.Name,
					Price = c.Price,
					ImageUrl = c.ImageUrl,
					Description = c.Description,
					IsAvailable = c.IsAvailable,
					CreatedAt = c.CreatedAt,
					DeletedAt = c.DeletedAt,
					AverageRating = c.Reviews != null && c.Reviews.Count > 0
									? (int)Math.Round(c.Reviews.Average(r => r.Rating))
									: 0
				})
				.ToListAsync();

			return Ok(result);
		}

		/// <summary>
		/// Lấy danh sách tất cả combo (Cho admin) và không bao gồm luôn comboFood.
		/// </summary>
		/// <returns>Danh sách combo.</returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ComboResponDto>>> GetAll()
		{
			var result = await _context.Combos
				.Select(c => new ComboResponDto
				{
					Id = c.Id,
					Name = c.Name,
					Price = c.Price,
					ImageUrl = c.ImageUrl,
					Description = c.Description,
					IsAvailable = c.IsAvailable,
					CreatedAt = c.CreatedAt,
					DeletedAt = c.DeletedAt,
					AverageRating = c.Reviews != null && c.Reviews.Count > 0
									? (int)Math.Round(c.Reviews.Average(r => r.Rating))
									: 0
				})
				.ToListAsync();

			return Ok(result);
		}


		// ================================================================
		// GET COMBO BY ID
		// ================================================================
		/// <summary>
		/// Lấy thông tin chi tiết của một combo theo ID.
		/// </summary>
		[HttpGet("{id}")]
        public async Task<ActionResult<ComboResponDto>> Get(int id)
        {
			var result = await _context.Combos
                .Include(c => c.ComboFoods)
                .Include(c => c.Reviews)
				.Where(c => c.DeletedAt == null && c.Id == id)
	            .Select(c => new ComboResponDto
	            {
					Id = c.Id,
					Name = c.Name,
					Price = c.Price,
					ImageUrl = c.ImageUrl,
					Description = c.Description,
					IsAvailable = c.IsAvailable,
					CreatedAt = c.CreatedAt,
					UpdatedAt = c.UpdatedAt,
					DeletedAt = c.DeletedAt,
					AverageRating = c.Reviews != null && c.Reviews.Count > 0
									? (int)Math.Round(c.Reviews.Average(r => r.Rating))
									: 0,

					ComboFoods = c.ComboFoods.Select(cf => new FoodInComboDto
		            {
			            FoodId = cf.FoodId,
			            Name = cf.Food.Name,
			            Quantity = cf.Quantity
		            }).ToList()
	            })
	            .FirstOrDefaultAsync();


			if (result == null)
                return NotFound();

			return Ok(result);
        }

        // ================================================================
        // CREATE COMBO
        // ================================================================
        /// <summary>
        /// Tạo combo mới (hỗ trợ upload hình ảnh).
        /// </summary>
        /// <remarks>
        /// Sử dụng <b>multipart/form-data</b> để upload ảnh.
        /// </remarks>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ComboCreateDto request)
        {
            string? imageUrl = await ImageHelper.UploadImageAsync(
                env: _env,
                file: request.FImageFile,
                folderName: "combo"
            );

			var combo = new Combo
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description ?? "No description",
                ImageUrl = imageUrl
            };

            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();

            if (request.ComboFoods != null)
            {
                foreach (var item in request.ComboFoods)
                {
                    _context.ComboFoods.Add(new ComboFood
                    {
                        ComboId = combo.Id,
                        FoodId = item.FoodId,
                        Quantity = item.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = combo.Id }, combo);
        }

        // ================================================================
        // UPDATE COMBO
        // ================================================================
        /// <summary>
        /// Cập nhật combo (cho phép đổi hình ảnh).
        /// </summary>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] ComboUpdateDto request)
        {
            var combo = await _context.Combos
                .Include(c => c.ComboFoods)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (combo == null)
                return NotFound();

            combo.Name = request.Name;
            combo.Price = request.Price;

            if (request.FImageFile != null)
            {
                combo.ImageUrl = await ImageHelper.UploadImageAsync(
				    env: HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>(),
				    file: request.FImageFile,
				    folderName: "combo"
			    );
			}

            if (request.ComboFoods != null)
            {
                _context.ComboFoods.RemoveRange(combo.ComboFoods);

                foreach (var f in request.ComboFoods)
                {
                    combo.ComboFoods.Add(new ComboFood
                    {
                        ComboId = combo.Id,
                        FoodId = f.FoodId,
                        Quantity = f.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ================================================================
        // DELETE COMBO (SOFT DELETE)
        // ================================================================
        /// <summary>
        /// Xóa combo (soft delete – không xóa khỏi DB).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var combo = await _context.Combos.FindAsync(id);

            if (combo == null)
                return NotFound();

            combo.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
