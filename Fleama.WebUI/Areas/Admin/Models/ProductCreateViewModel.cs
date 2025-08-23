using Fleama.Core.Entities;

namespace Fleama.WebUI.Areas.Admin.Models
{
    public class ProductCreateViewModel
    {        
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<IFormFile> Images { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }        
        public int BrandId { get; set; }        
        public Brand? Brand { get; set; }
    }
}