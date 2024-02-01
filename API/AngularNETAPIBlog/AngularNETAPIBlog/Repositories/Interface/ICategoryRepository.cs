using AngularNETAPIBlog.Models.Domain;

namespace AngularNETAPIBlog.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryAsync(Category category);
    }
}
