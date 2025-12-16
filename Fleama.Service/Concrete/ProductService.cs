using Fleama.Core.Entities;
using Fleama.Core.Enums;
using Fleama.Data;
using Fleama.Service.Abstract;
using Fleama.Shared.Dtos;
using Fleama.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Linq.Expressions;

namespace Fleama.Service.Concrete
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(DatabaseContext context) : base(context)
        {
        }

        public async Task<Product> CreateProductAsync(Product product, List<FileDto> imageFiles)
        {
            product.IsActive = true;
            // Status should be set by the controller (Approved for admin, Pending for users)
            // If not set, default to Approved
            if (product.ApproveStatus == 0)
                product.ApproveStatus = ProductApproveStatus.Approved;
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // product.Id oluşur

            if (imageFiles != null && imageFiles.Any())
            {
                var paths = FileHelper.SaveFiles(imageFiles, "Products");
                product.Images = paths.Select(p => new Image
                {
                    Url = p,
                    ReferenceId = product.Id,
                    ImageType = ImageType.Product
                }).ToList();

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }

            return product;
        }

        public async Task<Product?> EditProductAsync(int id, Product updatedProduct, List<FileDto> newImages, List<int>? removeImgIds = null)
        {
            var existingProduct = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
                return null;

            if (removeImgIds != null && removeImgIds.Any() && existingProduct.Images != null)
            {
                var imagesToRemove = existingProduct.Images.Where(x => removeImgIds.Contains(x.Id)).ToList();
                foreach (var img in imagesToRemove)
                    FileHelper.RemoveFile(img.Url);

                _context.Images.RemoveRange(imagesToRemove);
                existingProduct.Images = existingProduct.Images.Except(imagesToRemove).ToList();
            }

            if (newImages != null && newImages.Any())
            {
                var paths = FileHelper.SaveFiles(newImages, "Products");
                var imageEntities = paths.Select(p => new Image
                {
                    Url = p,
                    ReferenceId = existingProduct.Id,
                    ImageType = ImageType.Product
                }).ToList();

                if (existingProduct.Images == null)
                    existingProduct.Images = imageEntities;
                else
                    existingProduct.Images.AddRange(imageEntities);
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.ProductCode = updatedProduct.ProductCode;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.CategoryId = updatedProduct.CategoryId;
            existingProduct.BrandId = updatedProduct.BrandId;
            existingProduct.ApproveStatus = updatedProduct.ApproveStatus;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return existingProduct;
        }



        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return false;

            // Ürüne ait görselleri diskte sil
            if (product.Images != null)
            {
                foreach (var img in product.Images)
                    FileHelper.RemoveFile(img.Url);

                _context.Images.RemoveRange(product.Images);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> filter)
        {
            return await _context.Products
                                 .Where(filter)
                                 .Include(p => p.Images)
                                 .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByUserIdAsync(int userId)
        {
            return await _context.Products
                .Where(p => p.UserId == userId)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToListAsync();
        }

        public async Task<List<Product>> GetPendingProductsAsync()
        {
            return await _context.Products
                .Where(p => p.ApproveStatus == ProductApproveStatus.Pending)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<bool> UpdateProductStatusAsync(int productId, ProductApproveStatus status)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return false;

            product.ApproveStatus = status;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}