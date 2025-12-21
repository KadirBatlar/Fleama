using Fleama.Core.Entities;

namespace Fleama.Service.Abstract
{
    public interface IUserFavoriteService : IBaseService<UserFavoriteProduct>
    {
        /// <summary>
        /// Toggle favorite status for a product. If favorited, remove it. If not favorited, add it.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="productId">Product ID</param>
        /// <returns>True if added to favorites, False if removed from favorites</returns>
        Task<bool> ToggleFavoriteAsync(int userId, int productId);

        /// <summary>
        /// Check if a product is favorited by a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="productId">Product ID</param>
        /// <returns>True if favorited, False otherwise</returns>
        Task<bool> IsFavoriteAsync(int userId, int productId);

        /// <summary>
        /// Get all favorite products for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of favorite products</returns>
        Task<List<Product>> GetUserFavoritesAsync(int userId);

        /// <summary>
        /// Get the count of users who favorited a product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <returns>Count of favorites</returns>
        Task<int> GetFavoriteCountAsync(int productId);
    }
}
