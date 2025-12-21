using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Fleama.Service.Concrete
{
    public class UserFavoriteService : BaseService<UserFavoriteProduct>, IUserFavoriteService
    {
        public UserFavoriteService(DatabaseContext context) : base(context)
        {
        }

        public async Task<bool> ToggleFavoriteAsync(int userId, int productId)
        {
            // Check if the favorite already exists
            var existingFavorite = await _context.UserFavoriteProducts
                .FirstOrDefaultAsync(ufp => ufp.UserId == userId && ufp.ProductId == productId);

            if (existingFavorite != null)
            {
                // Remove from favorites
                _context.UserFavoriteProducts.Remove(existingFavorite);
                
                // Decrement favorite count on product
                var product = await _context.Products.FindAsync(productId);
                if (product != null && product.FavoriteCount > 0)
                {
                    product.FavoriteCount--;
                    _context.Products.Update(product);
                }

                await _context.SaveChangesAsync();
                return false; // Removed from favorites
            }
            else
            {
                // Add to favorites
                var favorite = new UserFavoriteProduct
                {
                    UserId = userId,
                    ProductId = productId,
                    IsActive = true
                };

                await _context.UserFavoriteProducts.AddAsync(favorite);

                // Increment favorite count on product
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.FavoriteCount++;
                    _context.Products.Update(product);
                }

                await _context.SaveChangesAsync();
                return true; // Added to favorites
            }
        }

        public async Task<bool> IsFavoriteAsync(int userId, int productId)
        {
            return await _context.UserFavoriteProducts
                .AnyAsync(ufp => ufp.UserId == userId && ufp.ProductId == productId);
        }

        public async Task<List<Product>> GetUserFavoritesAsync(int userId)
        {
            return await _context.UserFavoriteProducts
                .Where(ufp => ufp.UserId == userId)
                .Include(ufp => ufp.Product)
                    .ThenInclude(p => p.Images)
                .Include(ufp => ufp.Product)
                    .ThenInclude(p => p.Category)
                .Include(ufp => ufp.Product)
                    .ThenInclude(p => p.Brand)
                .OrderByDescending(ufp => ufp.CreatedDate)
                .Select(ufp => ufp.Product)
                .ToListAsync();
        }

        public async Task<int> GetFavoriteCountAsync(int productId)
        {
            return await _context.UserFavoriteProducts
                .CountAsync(ufp => ufp.ProductId == productId);
        }
    }
}
