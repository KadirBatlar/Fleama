namespace Fleama.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? ProductCode { get; set; }
        public bool IsHome { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int OrderNo { get; set; }
    }
}