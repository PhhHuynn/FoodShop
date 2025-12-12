using ASM.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly AppDbContext _context;
		public ProductsController(AppDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public async Task<ActionResult> GetProducts()
		{
			var products = await _context.Products
				.Where(p => p.DeletedAt == null)
				.ToListAsync();

			return Ok(products);
		}
	}
}
