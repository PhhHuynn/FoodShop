using ASM.Server.Data;
using ASM.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DashboardAdminController : ControllerBase
	{
		private readonly AppDbContext _context;

		public DashboardAdminController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetDashboardStats()
		{
			var totalUsers = await _context.Users.CountAsync();
			var totalFoods = await _context.Foods.CountAsync(); 
			var totalCombos = await _context.Combos.CountAsync();
			var totalCategories = await _context.Categories.CountAsync();

			var orders = await _context.Orders
				.GroupBy(o => o.Status) 
				.Select(g => new { Status = g.Key, Count = g.Count() })
				.ToListAsync();

			var orderStats = new
			{
				Pending = orders.FirstOrDefault(o => o.Status == OrderStatus.Pending)?.Count ?? 0,
				Shipping = orders.FirstOrDefault(o => o.Status == OrderStatus.Shipping)?.Count ?? 0,
				Delivered = orders.FirstOrDefault(o => o.Status == OrderStatus.Delivered)?.Count ?? 0
			};

			return Ok(new
			{
				TotalUsers = totalUsers,
				TotalFoods = totalFoods,
				TotalCombos = totalCombos,
				TotalCategories = totalCategories,
				Orders = orderStats
			});
		}
	}
}
