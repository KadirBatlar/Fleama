using Fleama.Core.Entities;
using Fleama.Core.Enums;
using Fleama.Shared.Dtos;

namespace Fleama.Service.Abstract
{
    public interface IProductService : IBaseService<Product>
    {
        Task<Product> CreateProductAsync(Product product, List<FileDto> images);
        Task<Product?> EditProductAsync(int id, Product updatedProduct, List<FileDto> newImages, List<int>? removeImgIds = null);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsByUserIdAsync(int userId);
        Task<List<Product>> GetPendingProductsAsync();
        Task<bool> UpdateProductStatusAsync(int productId, ProductApproveStatus status);
        Task<bool> ArchiveProductAsync(int productId, int userId);
        Task<bool> UnarchiveProductAsync(int productId, int userId);
        Task<List<Product>> GetArchivedProductsByUserIdAsync(int userId);

    }
}