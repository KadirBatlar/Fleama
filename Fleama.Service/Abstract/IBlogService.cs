using Fleama.Core.Entities;
using Fleama.Shared.Dtos;

namespace Fleama.Service.Abstract
{
    public interface IBlogService : IBaseService<BlogPost>
    {
        Task<IEnumerable<BlogPostDTO>> GetAllBlogPostsAsync();
        Task<BlogPostDTO?> GetBlogPostByIdAsync(int id);
        Task<BlogPostDTO> CreateBlogPostAsync(BlogCreateDTO createDto, FileDto? imageFile, int userId, bool isAdmin);
        Task<BlogPostDTO?> UpdateBlogPostAsync(int id, BlogUpdateDTO updateDto, FileDto? imageFile, int userId, bool isAdmin);
        Task<bool> DeleteBlogPostAsync(int id, int userId, bool isAdmin);
        Task<bool> CanUserEditOrDeleteAsync(int blogPostId, int userId, bool isAdmin);
    }
}

