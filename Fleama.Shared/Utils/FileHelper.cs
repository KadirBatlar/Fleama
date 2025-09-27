using Fleama.Shared.Dtos;

namespace Fleama.Shared.Utils;

public static class FileHelper
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    public static string? SaveFile(FileDto file, string folder)
    {
        if (file.Content.Length == 0 || file.Content.Length > MaxFileSize)
            return null;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return null;

        var newFileName = Guid.NewGuid().ToString("N") + extension;
        var saveFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", folder);
        Directory.CreateDirectory(saveFolder);

        var savePath = Path.Combine(saveFolder, newFileName);
        File.WriteAllBytes(savePath, file.Content);

        return $"/Img/{folder}/{newFileName}".Replace("\\", "/");
    }

    public static List<string> SaveFiles(IEnumerable<FileDto> files, string folder)
    {
        var paths = new List<string>();
        foreach (var file in files)
        {
            var path = SaveFile(file, folder);
            if (!string.IsNullOrEmpty(path))
                paths.Add(path);
        }
        return paths;
    }

    public static bool RemoveFile(string relativePath, string rootFolder = "wwwroot/Img")
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), rootFolder, relativePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }
        return false;
    }

    public static void RemoveFiles(IEnumerable<string> relativePaths, string rootFolder = "wwwroot/Img")
    {
        foreach (var path in relativePaths)
            RemoveFile(path, rootFolder);
    }
}