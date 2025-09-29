using Fleama.Core.Entities;
using Fleama.Shared.Dtos;

namespace Fleama.Service.Abstract
{
    public interface ICategoryService : IBaseService<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category, FileDto imageFile);
        Task<Category?> EditCategoryAsync(int id, Category updatedCategory, FileDto newImage, bool? removeImg);
        Task<bool> DeleteCategoryAsync(int id);
    }
}