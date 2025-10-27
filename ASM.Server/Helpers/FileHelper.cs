namespace ASM.Server.Helpers
{
	public class FileHelper
	{
		public static async Task<string> SaveFileAsync(IFormFile file, string folder)
		{
			var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
			var uploadDir = Path.Combine("wwwroot", folder);
			Directory.CreateDirectory(uploadDir);

			var filePath = Path.Combine(uploadDir, fileName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return $"/{folder}/{fileName}";
		}

		public static void DeleteFile(string relativePath)
		{
			var filePath = Path.Combine("wwwroot", relativePath.TrimStart('/'));
			if (File.Exists(filePath))
				File.Delete(filePath);
		}
	}
}
