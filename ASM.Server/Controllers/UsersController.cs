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

		private readonly UserManager<AppUser> _userManager;

		public UsersController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
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

		// PATCH: api/Users/5
		[HttpPatch("{id}")]
		public async Task<IActionResult> PatchUser(string id, [FromBody] UpdateUserDto model)
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

				if(hasPasword)
				{
					if(string.IsNullOrEmpty(model.OldPassword))
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
