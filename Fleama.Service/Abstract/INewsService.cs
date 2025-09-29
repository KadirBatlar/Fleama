using Fleama.Core.Entities;
using Fleama.Shared.Dtos;

namespace Fleama.Service.Abstract
{
    public interface INewsService : IBaseService<News>
    {
        Task<IEnumerable<News>> GetAllNewsAsync();
        Task<News> GetNewsByIdAsync(int id);
        Task<News> CreateNewsAsync(News news, FileDto imageFile);
        Task<News?> EditNewsAsync(int id, News updatedNews, FileDto newImage, bool? removeImg);
        Task<bool> DeleteNewsAsync(int id);
    }
}