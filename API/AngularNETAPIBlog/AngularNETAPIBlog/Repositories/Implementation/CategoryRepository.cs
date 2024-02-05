using AngularNETAPIBlog.API.Repositories.Interface;
using AngularNETAPIBlog.Data;
using AngularNETAPIBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AngularNETAPIBlog.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        Task<Category?> ICategoryRepository.GetCategoryByIdAsync(Guid id)
        {
            return _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        Task ICategoryRepository.UpdateCategoryAsync(Category category)
        {
            _dbContext.Categories.Update(category);
            return _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
