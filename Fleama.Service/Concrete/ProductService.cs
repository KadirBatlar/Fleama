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
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(DatabaseContext context) : base(context)
        {
        }

        public async Task<Product> CreateProductAsync(Product product, List<FileDto> imageFiles)
        {
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
            existingProduct.IsHome = updatedProduct.IsHome;
            existingProduct.CategoryId = updatedProduct.CategoryId;
            existingProduct.BrandId = updatedProduct.BrandId;
            existingProduct.OrderNo = updatedProduct.OrderNo;

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



    }
}