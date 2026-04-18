using AngularNETAPIBlog.Models.Domain;

namespace AngularNETAPIBlog.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();
        Task<BlogPost?> GetBlogPostByIdAsync(Guid id);
        Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle);
        Task UpdateBlogPostAsync(BlogPost blogPost);
        Task DeleteBlogPostAsync(BlogPost blogPost);
    }
}
