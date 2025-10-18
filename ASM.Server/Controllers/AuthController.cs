using ASM.Server.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASM.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : Controller
	{
		// Khi bắt tay vào làm giao diện sẽ tiếp tục với phần này

		private readonly IConfiguration _config;
		private readonly UserManager<IdentityUser> _userManager;

		public AuthController(UserManager<IdentityUser> userManager, IConfiguration config)
		{
			_userManager = userManager;
			_config = config;
		}

		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest model)
		{
			try
			{
				var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken);

				var user = await _userManager.FindByEmailAsync(payload.Email);

				if (user == null)
				{
					user = new IdentityUser
					{
						UserName = payload.Email,
						Email = payload.Email,
					};
					var result = await _userManager.CreateAsync(user);
					if (!result.Succeeded)
					{
						return BadRequest(new { message = "User creation failed.", errors = result.Errors });
					}
				}

				var claims = new[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Name, payload.Name),
				};

				var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(
					issuer: _config["Jwt:Issuer"],
					audience: _config["Jwt:Audience"],
					claims: claims,
					expires: DateTime.Now.AddHours(1),
					signingCredentials: creds
				);

				var jwt = new JwtSecurityTokenHandler().WriteToken(token);
				return Ok(new
				{
					token = jwt,
					user = new
					{
						id = user.Id,
						email = user.Email,
						name = payload.Name
					}
				});

			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Invalid Google token", error = ex.Message });
			}
		}
	}
}
