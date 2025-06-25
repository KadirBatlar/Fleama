using Fleama.Core.Entities;

namespace Fleama.Service.Abstract
{
    public interface ICategoryService : IBaseService<Category>
    {
        void UpdateCategoryWithoutModifyingImage(Category category);
    }
}