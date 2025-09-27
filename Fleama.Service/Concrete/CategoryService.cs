using Fleama.Core.Entities;
using Fleama.Core.Enums;
using Fleama.Data;
using Fleama.Service.Abstract;
using Fleama.Shared.Dtos;
using Fleama.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Fleama.Service.Concrete
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(x => x.Image)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            if (id == null)
                return null;

            return await _context.Categories
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Category> CreateCategoryAsync(Category category, FileDto imageFile)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync(); // Id oluşur

            if (imageFile != null )
            {
                var path = FileHelper.SaveFile(imageFile, "Categories");
                category.Image = new Image
                {
                    Url = path,
                    ReferenceId = category.Id,
                    ImageType = ImageType.Category
                };

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }

            return category;
        }
        public async Task<Category?> EditCategoryAsync(int id, Category updatedCategory, FileDto newImage, bool removeImg)
        {
            var existingCategory = await GetCategoryByIdAsync(id);

            if (existingCategory == null)
                return null;

            if (removeImg == true)
            {
                FileHelper.RemoveFile(existingCategory.Image.Url);
                _context.Images.Remove(existingCategory.Image);
            }

            if (newImage != null)
            {
                var path = FileHelper.SaveFile(newImage, "Categories");
                var imageEntities = new Image
                {
                    Url = path,
                    ReferenceId = existingCategory.Id,
                    ImageType = ImageType.Category
                };
                existingCategory.Image = imageEntities;
            }

            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;
            existingCategory.ImageId = updatedCategory.ImageId;
            existingCategory.IsTopMenu = updatedCategory.IsTopMenu;
            existingCategory.ParentId = updatedCategory.ParentId;
            existingCategory.OrderNo = updatedCategory.OrderNo;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);

            if (category == null)
                return false;

            // Remove image
            if (category.Image != null)
            {
                FileHelper.RemoveFile(category.Image.Url);
                _context.Images.RemoveRange(category.Image);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}