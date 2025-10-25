using ASM.Server.Models;

namespace ASM.Server.DTOs
{
	public class UpdateUserDto
	{
		public string? Id { get; set; }
		public string? FullName { get; set; }
		public string? Address { get; set; }
		public UserStatus? Status { get; set; }
		public string? NewPassword { get; set; }
		public string? OldPassword { get; set; }
		public string? Role { get; set; }
	}
}
