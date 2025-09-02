using Microsoft.AspNetCore.Http;

namespace Fleama.WebUI.Utils
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile formFile, string filePath = "/Img/")
        {
            string fileName = "";

            if (formFile != null && formFile.Length > 0)
            {
                fileName = formFile.FileName.ToLower();

                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await formFile.CopyToAsync(stream);
            }

            return fileName;
        }

        public static bool FileRemover(string fileName, string filePath = "/Img/")
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'), fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
    }
}
