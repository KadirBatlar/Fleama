namespace Fleama.Core.Entities
{
    public class News : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}