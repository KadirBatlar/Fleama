namespace Fleama.Shared.Dtos
{
    public class FileDto
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
    }
}