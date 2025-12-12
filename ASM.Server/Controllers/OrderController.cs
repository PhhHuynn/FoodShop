using ASM.Server.Dtos.OrderDtos;
using Microsoft.AspNetCore.Mvc;
using ASM.Server.Data;
using Microsoft.EntityFrameworkCore;
using ASM.Server.Models;
using System.Security.Claims;

namespace ASM.Server.Controllers
{
    /// <summary>
    /// API quản lý đơn hàng
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }

        // =====================================================================
        // GET: api/Order
        // =====================================================================

        /// <summary>
        /// Lấy danh sách tất cả đơn hàng
        /// </summary>
        /// <remarks>
        /// Trả về toàn bộ danh sách đơn hàng, kèm thông tin chi tiết, user và voucher
        /// </remarks>
        /// <returns>Danh sách các đơn hàng</returns>
        /// <response code="200">Thành công, trả về danh sách đơn hàng</response>
        /// <response code="404">Không tìm thấy đơn hàng</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
			
            var orders = await _db.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.User)
                .Include(o => o.Voucher)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    ShippingAddress = o.ShippingAddress,
                    Status = o.Status,
                    DiscountAmount = o.DiscountAmount,
                    TotalAmount = o.TotalAmount,
                    PaymentMethod = o.PaymentMethod,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                }).ToListAsync();

            return Ok(orders);
        }

        // =====================================================================
        // GET: api/Order/{id}
        // =====================================================================

        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng theo Id
        /// </summary>
        /// <param name="id">ID đơn hàng</param>
        /// <returns>Thông tin đơn hàng</returns>
        /// <response code="200">Thành công, trả về đơn hàng</response>
        /// <response code="404">Không tìm thấy đơn hàng</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _db.Orders
                 .Where(o => o.Id == id)
                 .Include(o => o.OrderDetails)
                 .Include(o => o.User)
                 .Include(o => o.Voucher)
                 .Select(o => new OrderDto
                 {
                     Id = o.Id,
					 UserId = o.UserId,
					 ShippingAddress = o.ShippingAddress,
                     Status = o.Status,
                     DiscountAmount = o.DiscountAmount,
                     TotalAmount = o.TotalAmount,
                     PaymentMethod = o.PaymentMethod,
                     CreatedAt = o.CreatedAt,
                     UpdatedAt = o.UpdatedAt
                 }).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

		/// <summary>
		/// Lấy danh sách tất cả đơn hàng
		/// </summary>
		/// <remarks>
		/// Trả về toàn bộ danh sách đơn hàng, kèm thông tin chi tiết, user và voucher
		/// </remarks>
		/// <returns>Danh sách các đơn hàng</returns>
		/// <response code="200">Thành công, trả về danh sách đơn hàng</response>
		/// <response code="404">Không tìm thấy đơn hàng</response>
		[HttpGet("me")]
		public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyAll()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized("User not logged in");

			var orders = await _db.Orders
				.Include(o => o.OrderDetails)
				.Include(o => o.User)
				.Include(o => o.Voucher)
                .Where(o => o.UserId == userId)
				.Select(o => new OrderDto
				{
					Id = o.Id,
					ShippingAddress = o.ShippingAddress,
					Status = o.Status,
					DiscountAmount = o.DiscountAmount,
					TotalAmount = o.TotalAmount,
					PaymentMethod = o.PaymentMethod,
					CreatedAt = o.CreatedAt,
					UpdatedAt = o.UpdatedAt
				}).ToListAsync();

			return Ok(orders);
		}

		// =====================================================================
		// POST: api/Order
		// =====================================================================

		/// <summary>
		/// Tạo mới đơn hàng
		/// </summary>
		/// <param name="dto">Thông tin đơn hàng cần tạo</param>
		/// <remarks>
		/// Khi tạo đơn hàng mới, trạng thái mặc định là Pending.
		/// Có thể thêm chi tiết đơn hàng kèm theo
		/// </remarks>
		/// <returns>Thông tin đơn hàng vừa tạo</returns>
		/// <response code="201">Tạo đơn hàng thành công</response>
		/// <response code="400">Dữ liệu không hợp lệ</response>
		[HttpPost]
        public async Task<ActionResult<OrderCreateDto>> Post([FromBody] OrderCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized("User not logged in");

			var order = new Order
            {
                ShippingAddress = dto.ShippingAddress,
                Status = OrderStatus.Pending,
                PaymentMethod = dto.PaymentMethod,
                DiscountAmount = dto.DiscountAmount,
                TotalAmount = dto.TotalAmount,
                UserId = userId,
                VoucherId = dto.VoucherId,
                CreatedAt = DateTime.UtcNow,
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            if (dto.OrderDetails != null && dto.OrderDetails.Count > 0)
            {
                foreach (var detailDto in dto.OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = detailDto.ProductId,
                        Quantity = detailDto.Quantity,
                        UnitPrice = detailDto.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                }
                await _db.SaveChangesAsync();
            }

            var result = await GetById(order.Id);
            return result.Result;
        }

        // =====================================================================
        // PUT: api/Order/{id}
        // =====================================================================

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        /// <param name="id">ID đơn hàng cần cập nhật</param>
        /// <param name="dto">Thông tin cập nhật (chỉ cập nhật trạng thái)</param>
        /// <returns>Thông tin đơn hàng sau khi cập nhật</returns>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="404">Không tìm thấy đơn hàng</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderUpdateDto dto)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = dto.Status;
            order.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var result = await GetById(order.Id);
            return Ok(result.Value);
        }
    }
}
