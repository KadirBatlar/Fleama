using Fleama.Shared.Dtos;

namespace Fleama.WebUI.Utils
{
    public static class FileMapper
    {
        public static async Task<FileDto> ToFileDtoAsync(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return new FileDto { FileName = file.FileName, Content = ms.ToArray() };
        }
    }
}