using ASM.Server.Models;

namespace ASM.Server.Dtos.UserDtos
{
	public class UserUpdateDto
	{
		public string? Id { get; set; }
		public string? FullName { get; set; }
		public string? Address { get; set; }
		public UserStatus? Status { get; set; }
		public string? Role { get; set; }
	}
}
