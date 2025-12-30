using Fleama.Core.Entities;
using Fleama.Core.Enums;

namespace Fleama.Service.Abstract
{
    public interface IProductInteractionService : IBaseService<ProductInteraction>
    {
        /// <summary>
        /// Create a new interaction request (swap or borrow)
        /// </summary>
        /// <param name="requesterUserId">User requesting the interaction</param>
        /// <param name="ownerUserId">Product owner user</param>
        /// <param name="productId">Product ID</param>
        /// <param name="interactionType">Type of interaction (swap or borrow)</param>
        /// <returns>Created interaction</returns>
        Task<ProductInteraction> CreateInteractionAsync(int requesterUserId, int ownerUserId, int productId, InteractionType interactionType);

        /// <summary>
        /// Accept a pending interaction request
        /// </summary>
        /// <param name="interactionId">Interaction ID</param>
        /// <param name="userId">User accepting (must be the owner)</param>
        /// <returns>True if successfully accepted</returns>
        Task<bool> AcceptInteractionAsync(int interactionId, int userId);

        /// <summary>
        /// Complete an accepted interaction
        /// </summary>
        /// <param name="interactionId">Interaction ID</param>
        /// <param name="userId">User completing the interaction</param>
        /// <returns>True if successfully completed</returns>
        Task<bool> CompleteInteractionAsync(int interactionId, int userId);

        /// <summary>
        /// Cancel an interaction
        /// </summary>
        /// <param name="interactionId">Interaction ID</param>
        /// <param name="userId">User cancelling (requester or owner)</param>
        /// <returns>True if successfully cancelled</returns>
        Task<bool> CancelInteractionAsync(int interactionId, int userId);

        /// <summary>
        /// Get all interactions requested by a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of requested interactions</returns>
        Task<List<ProductInteraction>> GetUserRequestedInteractionsAsync(int userId);

        /// <summary>
        /// Get all interactions received by a user (as product owner)
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of received interactions</returns>
        Task<List<ProductInteraction>> GetUserReceivedInteractionsAsync(int userId);

        /// <summary>
        /// Get all interactions for a specific product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <returns>List of product interactions</returns>
        Task<List<ProductInteraction>> GetProductInteractionsAsync(int productId);
    }
}
