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
	public class AccountController : ControllerBase
	{

		private readonly IConfiguration _config;

		private readonly UserManager<AppUser> _userManager;

		public AccountController(UserManager<AppUser> userManager, IConfiguration config)
		{
			_userManager = userManager;
			_config = config;
		}

		// POST: api/Auth/google-login
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

					await _userManager.AddToRoleAsync(user, "User");
				}

				var userRoles = await _userManager.GetRolesAsync(user);

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Name, payload.Name),
				};

				foreach (var role in userRoles)
				{
					claims.Add(new Claim(ClaimTypes.Role, role));
				}


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

		// POST: api/account/login
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto model)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					return Unauthorized(new { message = "Invalid email or password." });
				}

				var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
				if (!passwordValid)
				{
					return Unauthorized(new { message = "Invalid email or password." });
				}

				var userRoles = await _userManager.GetRolesAsync(user);

				var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Name, user.FullName),
			};

				foreach (var role in userRoles)
				{
					claims.Add(new Claim(ClaimTypes.Role, role));
				}

				var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(
					issuer: _config["Jwt:Issuer"],
					audience: _config["Jwt:Audience"],
					claims: claims,
					expires: DateTime.Now.AddHours(1),
					signingCredentials: creds
				);

				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					user = new
					{
						id = user.Id,
						email = user.Email,
						name = user.FullName
					}
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "An error occurred during login.", error = ex.Message });
			}
		}


		// PATCH: api/account/5
		[HttpPatch("{id}")]
		public async Task<IActionResult> PatchAccount(string id, [FromBody] UpdateUserDto model)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return NotFound();

			if (!string.IsNullOrEmpty(model.FullName))
				user.FullName = model.FullName;

			if (!string.IsNullOrEmpty(model.Address))
				user.Address = model.Address;

			if (model.Status.HasValue)
				user.Status = model.Status.Value;

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
				return BadRequest(result.Errors);

			if (!string.IsNullOrEmpty(model.NewPassword))
			{
				var hasPasword = await _userManager.HasPasswordAsync(user);
				IdentityResult passwordResult;

				if (hasPasword)
				{
					if (string.IsNullOrEmpty(model.OldPassword))
					{
						return BadRequest(new { message = "Old password is required to change the password." });
					}
					passwordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
				}
				else
				{
					passwordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
				}

				if (!passwordResult.Succeeded)
				{
					return BadRequest(passwordResult.Errors);
				}
			}

			

			return Ok(new { message = "User updated successfully." });
		}



	}
}
