using Fleama.Core.Entities;
using Fleama.Core.Enums;
using Fleama.Data;
using Fleama.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Fleama.Service.Concrete
{
    public class ProductInteractionService : BaseService<ProductInteraction>, IProductInteractionService
    {
        public ProductInteractionService(DatabaseContext context) : base(context)
        {
        }

        public async Task<ProductInteraction> CreateInteractionAsync(int requesterUserId, int ownerUserId, int productId, InteractionType interactionType)
        {
            var interaction = new ProductInteraction
            {
                RequesterUserId = requesterUserId,
                OwnerUserId = ownerUserId,
                ProductId = productId,
                InteractionType = interactionType,
                Status = InteractionStatus.Requested,
                IsActive = true
            };

            await _context.ProductInteractions.AddAsync(interaction);
            await _context.SaveChangesAsync();

            return interaction;
        }

        public async Task<bool> AcceptInteractionAsync(int interactionId, int userId)
        {
            var interaction = await _context.ProductInteractions
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == interactionId);

            if (interaction == null)
                return false;

            // Only the owner can accept
            if (interaction.OwnerUserId != userId)
                return false;

            // Can only accept if status is Requested
            if (interaction.Status != InteractionStatus.Requested)
                return false;

            // Update interaction status
            interaction.Status = InteractionStatus.Accepted;
            _context.ProductInteractions.Update(interaction);

            // Update product state to InTransaction
            if (interaction.Product != null)
            {
                interaction.Product.State = ProductState.InTransaction;
                _context.Products.Update(interaction.Product);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CompleteInteractionAsync(int interactionId, int userId)
        {
            var interaction = await _context.ProductInteractions
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == interactionId);

            if (interaction == null)
                return false;

            // Either requester or owner can complete
            if (interaction.RequesterUserId != userId && interaction.OwnerUserId != userId)
                return false;

            // Can only complete if status is Accepted
            if (interaction.Status != InteractionStatus.Accepted)
                return false;

            // Update interaction status
            interaction.Status = InteractionStatus.Completed;
            interaction.CompletedAt = DateTime.UtcNow;
            _context.ProductInteractions.Update(interaction);

            // Update product state to Completed
            if (interaction.Product != null)
            {
                interaction.Product.State = ProductState.Completed;
                _context.Products.Update(interaction.Product);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelInteractionAsync(int interactionId, int userId)
        {
            var interaction = await _context.ProductInteractions
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == interactionId);

            if (interaction == null)
                return false;

            // Either requester or owner can cancel
            if (interaction.RequesterUserId != userId && interaction.OwnerUserId != userId)
                return false;

            // Can cancel if not already Completed or Cancelled
            if (interaction.Status == InteractionStatus.Completed || interaction.Status == InteractionStatus.Cancelled)
                return false;

            // Update interaction status
            interaction.Status = InteractionStatus.Cancelled;
            _context.ProductInteractions.Update(interaction);

            // Restore product state to Available if it was InTransaction
            if (interaction.Product != null && interaction.Product.State == ProductState.InTransaction)
            {
                interaction.Product.State = ProductState.Available;
                _context.Products.Update(interaction.Product);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ProductInteraction>> GetUserRequestedInteractionsAsync(int userId)
        {
            return await _context.ProductInteractions
                .Where(i => i.RequesterUserId == userId)
                .Include(i => i.Product)
                    .ThenInclude(p => p!.Images)
                .Include(i => i.Product)
                    .ThenInclude(p => p!.Category)
                .Include(i => i.OwnerUser)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<ProductInteraction>> GetUserReceivedInteractionsAsync(int userId)
        {
            return await _context.ProductInteractions
                .Where(i => i.OwnerUserId == userId)
                .Include(i => i.Product)
                    .ThenInclude(p => p!.Images)
                .Include(i => i.Product)
                    .ThenInclude(p => p!.Category)
                .Include(i => i.RequesterUser)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<ProductInteraction>> GetProductInteractionsAsync(int productId)
        {
            return await _context.ProductInteractions
                .Where(i => i.ProductId == productId)
                .Include(i => i.RequesterUser)
                .Include(i => i.OwnerUser)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();
        }
    }
}
