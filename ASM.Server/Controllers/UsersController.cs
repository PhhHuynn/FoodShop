using ASM.Server.DTOs;
using ASM.Server.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASM.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IConfiguration _config;

		private readonly UserManager<AppUser> _userManager;

		public UsersController(UserManager<AppUser> userManager, IConfiguration config)
		{
			_userManager = userManager;
			_config = config;
		}

		// POST: api/Users/google-login
		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest model)
		{
			try
			{
				var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken);

				var user = await _userManager.FindByEmailAsync(payload.Email);

				if (user == null)
				{
					user = new AppUser
					{
						UserName = payload.Email,
						Email = payload.Email,
						FullName = payload.Name,
						Address = ""
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

		// GET: api/Users
		[HttpGet]
		public IActionResult GetUsers()
		{
			var users = _userManager.Users.ToList();
			return Ok(users);
		}

		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}

		// POST: api/Users
		[HttpPost]
		public async Task<IActionResult> PostUser([FromBody] RegisterDto model)
		{
			var user = new AppUser
			{
				UserName = model.Email,
				Email = model.Email,
				FullName = model.FullName,
				Address = model.Address,
				Status = model.Status,
				CreateAt = DateTime.UtcNow
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			
			if (!string.IsNullOrEmpty(model.Role))
			{
				await _userManager.AddToRoleAsync(user, model.Role);
			}

			return Ok(new { message = "User created successfully by admin." });
		}

		// PUT: api/Users/5
		[HttpPut]
		public async Task<IActionResult> PutUser([FromBody] UpdateUserDto model)
		{
			var user = await _userManager.FindByIdAsync(model.Id);
			if (user == null)
			{
				return NotFound();
			}
			user.FullName = model.FullName;
			user.Address = model.Address;
			user.Status = model.Status;
			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok(new { message = "User updated successfully." });
		}


		// DELETE: api/Users/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			var result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok(new { message = "User deleted successfully." });
		}
	}
}
