using Fleama.Core.Entities;

namespace Fleama.Service.Abstract
{
    public interface IProductService : IBaseService<Product>
    {
        Task<List<Product>> GetAllWithBrandAndCategoryAsync();
        Task<Product> GetByIdWithBrandAndCategoryAsync(int id);

    }
}