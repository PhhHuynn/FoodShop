using ASM.Server.Data;
using ASM.Server.Dtos.ReviewDtos;
using ASM.Server.Helpers;
using ASM.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

		// GET: api/Reviews
		/// <summary>
		/// Lấy danh sách tất cả các đánh giá.
		/// </summary>
		/// <returns>
		/// Danh sách các đánh giá, bao gồm thông tin người dùng và thời gian tạo.
		/// </returns>
		/// <response code="200">Trả về danh sách đánh giá</response>
		[HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            var reviews = await _context.Reviews.Include(r => r.User).Select(r => new ReviewDto(r)).ToListAsync();
			return Ok(reviews);
        }

		// GET: api/Reviews/5
		/// <summary>
		/// Lấy chi tiết một đánh giá theo ID.
		/// </summary>
		/// <param name="id">ID của đánh giá cần lấy</param>
		/// <returns>Chi tiết đánh giá tương ứng với ID.</returns>
		/// <response code="200">Trả về thông tin đánh giá</response>
		/// <response code="404">Không tìm thấy đánh giá</response>
		[HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var existingReview = await _context.Reviews.FindAsync(id);

            if (existingReview == null)
            {
                return NotFound();
            }

            return Ok(new ReviewDto(existingReview));
        }

		/// <summary>
		/// Lấy danh sách đánh giá theo sản phẩm.
		/// </summary>
		/// <param name="productId">ID sản phẩm cần lấy danh sách đánh giá</param>
		/// <returns>Danh sách các đánh giá thuộc sản phẩm.</returns>
		/// <response code="200">Trả về danh sách đánh giá</response>
		[HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByProduct(int productId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId && r.DeletedAt == null)
                .Select(r => new ReviewDto(r))
				.ToListAsync();
            return Ok(reviews);
		}

		/// <summary>
		/// Lấy danh sách đánh giá của người dùng đang đăng nhập.
		/// </summary>
		/// <returns>Danh sách các đánh giá mà người dùng đã tạo. 
        /// Chỉ trả về đánh giá chưa bị xóa (DeletedAt = null)</returns>
		/// <response code="200">Trả về danh sách đánh giá</response>
		[HttpGet("me")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMyReviews()
        {
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized("User not logged in");

			var reviews = await _context.Reviews
                .Where(r => r.UserId == userId && r.DeletedAt == null)
                .Select(r => new ReviewDto(r))
				.ToListAsync();
            return Ok(reviews);
		}

		// PUT: api/Reviews/5
		/// <summary>
		/// Cập nhật nội dung của một đánh giá.
		/// </summary>
		/// <param name="id">ID của đánh giá cần cập nhật</param>
		/// <param name="review">Dữ liệu mới để cập nhật đánh giá</param>
		/// <returns>Trả về đánh giá sau khi cập nhật.</returns>
		/// <response code="200">Cập nhật thành công và trả về dữ liệu mới</response>
		/// <response code="400">Không thể cập nhật vì đánh giá đã bị xóa</response>
		/// <response code="404">Không tìm thấy đánh giá</response>
		[HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, ReviewUpdateDto review)
        {
            var existingReview = await _context.Reviews.FindAsync(id);
            if (existingReview == null)
                return NotFound();

            if (existingReview.DeletedAt != null)
                return BadRequest("Cannot update a deleted review.");

            if (DateTime.UtcNow - existingReview.CreatedAt > TimeSpan.FromHours(24)) {
                return BadRequest("Cannot update review after 24 hours of creation.");
			}

				existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            existingReview.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok ( new ReviewDto(existingReview));
        }

		// POST: api/Reviews
		/// <summary>
		/// Tạo một đánh giá mới.
		/// </summary>
		/// <param name="review">Thông tin đánh giá mới</param>
		/// <returns>Đánh giá vừa tạo</returns>
		/// <response code="201">Đã tạo thành công</response>
		[HttpPost]
		public async Task<ActionResult<ReviewDto>> PostReview(ReviewCreateDto review)
        {
            var newReview = new Review
            {
                ProductId = review.ProductId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = DateTime.UtcNow
            };
			_context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();

			var reviewDto = new ReviewDto(newReview);

			return CreatedAtAction("GetReview", new { id = newReview.Id }, reviewDto);
		}

		// DELETE: api/Reviews/5
		/// <summary>
		/// Xóa một đánh giá theo ID.
		/// </summary>
		/// <remarks>
		/// Hành động này chỉ đánh dấu đánh giá là đã bị xóa (soft delete),
		/// không xóa hoàn toàn khỏi cơ sở dữ liệu.
		/// </remarks>
		/// <param name="id">ID của đánh giá cần xóa</param>
		/// <returns>Không có nội dung.</returns>
		/// <response code="204">Xóa thành công</response>
		/// <response code="404">Không tìm thấy đánh giá</response>
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

			review.DeletedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;

            _context.Reviews.Update(review);
			await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
	}
}
