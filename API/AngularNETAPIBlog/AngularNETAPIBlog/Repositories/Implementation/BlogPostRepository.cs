using AngularNETAPIBlog.API.Repositories.Interface;
using AngularNETAPIBlog.Data;
using AngularNETAPIBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AngularNETAPIBlog.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            return await dbContext.BlogPosts
                .OrderByDescending(x => x.PublishedDate)
                .ToListAsync();
        }

        public Task<BlogPost?> GetBlogPostByIdAsync(Guid id)
        {
            return dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle)
        {
            return dbContext.BlogPosts.FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            dbContext.BlogPosts.Update(blogPost);
            return dbContext.SaveChangesAsync();
        }

        public async Task DeleteBlogPostAsync(BlogPost blogPost)
        {
            dbContext.BlogPosts.Remove(blogPost);
            await dbContext.SaveChangesAsync();
        }
    }
}
