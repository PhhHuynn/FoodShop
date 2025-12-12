using ASM.Server.Models;

namespace ASM.Server.Dtos.UserDtos
{
	public class ProfileUpdateDto
	{
		public string? Id { get; set; }
		public string? FullName { get; set; }
		public string? Address { get; set; }
		public string? PasswordOld { get; set; }
		public string? PasswordNew { get; set; }
	}
}
