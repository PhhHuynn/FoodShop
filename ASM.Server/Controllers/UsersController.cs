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
		public async Task<IActionResult> GetUsers()
		{
			var users = _userManager.Users.ToList();

			var usersWithRoles = new List<object>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				usersWithRoles.Add(new
				{
					user.Id,
					user.Email,
					user.FullName,
					user.Address,
					user.Status,
					Role = roles.FirstOrDefault()
				});
			}

			return Ok(usersWithRoles);
		}


		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return NotFound();

			var roles = await _userManager.GetRolesAsync(user);

			var userWithRole = new
			{
				user.Id,
				user.Email,
				user.FullName,
				user.Address,
				user.Status,
				Role = roles.FirstOrDefault()
			};

			return Ok(userWithRole);
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
		public async Task<IActionResult> PatchUser(string id, [FromBody] UserUpdateDto model)
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

			if (!string.IsNullOrEmpty(model.Role))
			{
				var currentRoles = await _userManager.GetRolesAsync(user);
				var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
				if (!removeResult.Succeeded)
				{
					return BadRequest(removeResult.Errors);
				}
				var addResult = await _userManager.AddToRoleAsync(user, model.Role);
				if (!addResult.Succeeded)
				{
					return BadRequest(addResult.Errors);
				}
			}

			var updateResult = await _userManager.UpdateAsync(user);
			if (!updateResult.Succeeded)
				return BadRequest(updateResult.Errors);


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
