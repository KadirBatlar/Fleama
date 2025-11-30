using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.Service.Abstract;
using Fleama.Shared.Dtos;
using Fleama.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Fleama.Service.Concrete
{
    public class BlogService : BaseService<BlogPost>, IBlogService
    {
        public BlogService(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BlogPostDTO>> GetAllBlogPostsAsync()
        {
            var posts = await _context.BlogPosts
                .Include(b => b.User)
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .AsNoTracking()
                .ToListAsync();

            return posts.Select(p => new BlogPostDTO
            {
                Id = p.Id,
                UserId = p.UserId,
                UserName = p.User?.UserName ?? "Bilinmeyen",
                UserFullName = $"{p.User?.Name} {p.User?.Surname}".Trim(),
                Title = p.Title,
                Content = p.Content,
                ImageUrl = p.ImageUrl,
                IsVerified = p.IsVerified,
                CreatedDate = p.CreatedDate,
                UpdatedAt = p.UpdatedAt,
                IsActive = p.IsActive
            });
        }

        public async Task<BlogPostDTO?> GetBlogPostByIdAsync(int id)
        {
            var post = await _context.BlogPosts
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);

            if (post == null)
                return null;

            return new BlogPostDTO
            {
                Id = post.Id,
                UserId = post.UserId,
                UserName = post.User?.UserName ?? "Bilinmeyen",
                UserFullName = $"{post.User?.Name} {post.User?.Surname}".Trim(),
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                IsVerified = post.IsVerified,
                CreatedDate = post.CreatedDate,
                UpdatedAt = post.UpdatedAt,
                IsActive = post.IsActive
            };
        }

        public async Task<BlogPostDTO> CreateBlogPostAsync(BlogCreateDTO createDto, FileDto? imageFile, int userId, bool isAdmin)
        {
            var blogPost = new BlogPost
            {
                Title = createDto.Title,
                Content = createDto.Content,
                UserId = userId,
                IsVerified = isAdmin, // Admin posts are automatically verified
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            // Handle image upload
            if (imageFile != null && imageFile.Content.Length > 0)
            {
                var imagePath = SaveBlogImage(imageFile);
                blogPost.ImageUrl = imagePath;
            }

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            // Return DTO
            var user = await _context.AppUsers.FindAsync(userId);
            return new BlogPostDTO
            {
                Id = blogPost.Id,
                UserId = blogPost.UserId,
                UserName = user?.UserName ?? "Bilinmeyen",
                UserFullName = $"{user?.Name} {user?.Surname}".Trim(),
                Title = blogPost.Title,
                Content = blogPost.Content,
                ImageUrl = blogPost.ImageUrl,
                IsVerified = blogPost.IsVerified,
                CreatedDate = blogPost.CreatedDate,
                UpdatedAt = blogPost.UpdatedAt,
                IsActive = blogPost.IsActive
            };
        }

        public async Task<BlogPostDTO?> UpdateBlogPostAsync(int id, BlogUpdateDTO updateDto, FileDto? imageFile, int userId, bool isAdmin)
        {
            var existingPost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingPost == null)
                return null;

            // Check authorization
            if (!isAdmin && existingPost.UserId != userId)
                return null;

            existingPost.Title = updateDto.Title;
            existingPost.Content = updateDto.Content;
            existingPost.UpdatedAt = DateTime.Now;

            // Handle image update
            if (imageFile != null && imageFile.Content.Length > 0)
            {
                // Remove old image if exists
                if (!string.IsNullOrEmpty(existingPost.ImageUrl))
                {
                    RemoveBlogImage(existingPost.ImageUrl);
                }

                existingPost.ImageUrl = SaveBlogImage(imageFile);
            }

            _context.BlogPosts.Update(existingPost);
            await _context.SaveChangesAsync();

            // Return updated DTO
            var user = await _context.AppUsers.FindAsync(existingPost.UserId);
            return new BlogPostDTO
            {
                Id = existingPost.Id,
                UserId = existingPost.UserId,
                UserName = user?.UserName ?? "Bilinmeyen",
                UserFullName = $"{user?.Name} {user?.Surname}".Trim(),
                Title = existingPost.Title,
                Content = existingPost.Content,
                ImageUrl = existingPost.ImageUrl,
                IsVerified = existingPost.IsVerified,
                CreatedDate = existingPost.CreatedDate,
                UpdatedAt = existingPost.UpdatedAt,
                IsActive = existingPost.IsActive
            };
        }

        public async Task<bool> DeleteBlogPostAsync(int id, int userId, bool isAdmin)
        {
            var post = await _context.BlogPosts.FindAsync(id);

            if (post == null)
                return false;

            // Check authorization
            if (!isAdmin && post.UserId != userId)
                return false;

            // Remove image if exists
            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                RemoveBlogImage(post.ImageUrl);
            }

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CanUserEditOrDeleteAsync(int blogPostId, int userId, bool isAdmin)
        {
            if (isAdmin)
                return true;

            var post = await _context.BlogPosts.FindAsync(blogPostId);
            return post != null && post.UserId == userId;
        }

        private string SaveBlogImage(FileDto fileDto)
        {
            if (fileDto.Content.Length == 0)
                return string.Empty;

            var extension = Path.GetExtension(fileDto.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            
            if (!allowedExtensions.Contains(extension))
                return string.Empty;

            var newFileName = Guid.NewGuid().ToString("N") + extension;
            var saveFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blog");
            Directory.CreateDirectory(saveFolder);

            var savePath = Path.Combine(saveFolder, newFileName);
            File.WriteAllBytes(savePath, fileDto.Content);

            return $"/uploads/blog/{newFileName}";
        }

        private void RemoveBlogImage(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}

