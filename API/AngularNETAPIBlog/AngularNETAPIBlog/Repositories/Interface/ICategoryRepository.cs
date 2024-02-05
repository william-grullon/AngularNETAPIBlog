using AngularNETAPIBlog.Models.Domain;

namespace AngularNETAPIBlog.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task UpdateCategoryAsync(Category category);
    }
}
