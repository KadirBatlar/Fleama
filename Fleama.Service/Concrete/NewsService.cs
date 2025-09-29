using Fleama.Core.Entities;
using Fleama.Core.Enums;
using Fleama.Data;
using Fleama.Service.Abstract;
using Fleama.Shared.Dtos;
using Fleama.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Fleama.Service.Concrete
{
    public class NewsService : BaseService<News>, INewsService
    {
        public NewsService(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<News>> GetAllNewsAsync()
        {
            return await _context.News
                .Include(x => x.Image)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<News> GetNewsByIdAsync(int id)
        {
            if (id == null)
                return null;

            return await _context.News
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<News> CreateNewsAsync(News news, FileDto imageFile)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync(); // Id oluşur

            if (imageFile != null)
            {
                var path = FileHelper.SaveFile(imageFile, "News");
                news.Image = new Image
                {
                    Url = path,
                    ReferenceId = news.Id,
                    ImageType = ImageType.Category
                };

                _context.News.Update(news);
                await _context.SaveChangesAsync();
            }

            return news;
        }

        public async Task<News?> EditNewsAsync(int id, News updatedNews, FileDto newImage, bool? removeImg)
        {
            var existingNews = await GetNewsByIdAsync(id);

            if (existingNews == null)
                return null;

            if (removeImg == true)
            {
                FileHelper.RemoveFile(existingNews.Image.Url);
                _context.Images.Remove(existingNews.Image);
            }

            if (newImage != null && newImage.Content.Length > 0)
            {
                var path = FileHelper.SaveFile(newImage, "News");
                var imageEntities = new Image
                {
                    Url = path,
                    ReferenceId = existingNews.Id,
                    ImageType = ImageType.News
                };
                existingNews.Image = imageEntities;
            }

            existingNews.Name = updatedNews.Name;
            existingNews.Description = updatedNews.Description;
            existingNews.ImageId = updatedNews.ImageId;

            _context.News.Update(existingNews);
            await _context.SaveChangesAsync();

            return existingNews;
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await GetNewsByIdAsync(id);

            if (news == null)
                return false;

            // Remove image
            if (news.Image != null)
            {
                FileHelper.RemoveFile(news.Image.Url);
                _context.Images.RemoveRange(news.Image);
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}