using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Fleama.Service.Concrete
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(DatabaseContext context) : base(context)
        {
        }

        public void UpdateCategoryWithoutModifyingImage(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            _context.Entry(category).Property(x => x.Image).IsModified = false;
            _context.SaveChanges();
        }
    }
}