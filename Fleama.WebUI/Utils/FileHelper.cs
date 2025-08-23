namespace Fleama.WebUI.Utils
{
    public static class FileHelper
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public static async Task<string?> FileLoaderAsync(IFormFile formFile, string folder = "Img")
        {
            if (formFile == null || formFile.Length == 0)
                return null;

            if (formFile.Length > MaxFileSize)
                return null;

            var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return null;

            var newFileName = Guid.NewGuid().ToString("N") + extension;
            var saveFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            var savePath = Path.Combine(saveFolder, newFileName);

            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            using var stream = new FileStream(savePath, FileMode.Create);
            await formFile.CopyToAsync(stream);

            // wwwroot sonrası yolu dön (örn: /Img/abcd123.png)
            return $"/{folder}/{newFileName}".Replace("\\", "/");
        }

        public static async Task<List<string>> FileLoaderMultipleAsync(IEnumerable<IFormFile> files, string folder = "Img")
        {
            var savedFiles = new List<string>();
            foreach (var file in files)
            {
                var result = await FileLoaderAsync(file, folder);
                if (!string.IsNullOrEmpty(result))
                    savedFiles.Add(result);
            }
            return savedFiles;
        }

        public static bool FileRemover(string relativePath, string rootFolder = "wwwroot")
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return false;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), rootFolder, relativePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }

        public static void FileRemoverMultiple(IEnumerable<string> relativePaths, string rootFolder = "wwwroot")
        {
            foreach (var file in relativePaths)
            {
                FileRemover(file, rootFolder);
            }
        }
    }
}