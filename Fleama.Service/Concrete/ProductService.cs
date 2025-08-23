using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Fleama.Service.Concrete
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(DatabaseContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetAllWithBrandAndCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();
        }
        public async Task<Product> GetByIdWithBrandAndCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}