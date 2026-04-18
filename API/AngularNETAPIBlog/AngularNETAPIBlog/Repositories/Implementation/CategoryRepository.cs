using AngularNETAPIBlog.API.Repositories.Interface;
using AngularNETAPIBlog.Models.Domain;

namespace AngularNETAPIBlog.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string categoriesDirectory;

        public CategoryRepository(IWebHostEnvironment environment)
        {
            categoriesDirectory = Path.Combine(environment.ContentRootPath, "content", "categories");
            Directory.CreateDirectory(categoriesDirectory);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if (category.Id == Guid.Empty)
            {
                category.Id = Guid.NewGuid();
            }

            await File.WriteAllTextAsync(GetFilePath(category.Id), BuildMarkdown(category));
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = new List<Category>();

            foreach (var filePath in Directory.EnumerateFiles(categoriesDirectory, "*.md"))
            {
                categories.Add(await ReadCategoryAsync(filePath));
            }

            return categories.OrderBy(category => category.Name);
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            var filePath = GetFilePath(id);
            if (!File.Exists(filePath))
            {
                return null;
            }

            return await ReadCategoryAsync(filePath);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await File.WriteAllTextAsync(GetFilePath(category.Id), BuildMarkdown(category));
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            var filePath = GetFilePath(category.Id);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private async Task<Category> ReadCategoryAsync(string filePath)
        {
            var markdown = await File.ReadAllTextAsync(filePath);
            var (frontMatter, _) = MarkdownFrontMatter.Parse(markdown);

            return new Category
            {
                Id = ParseGuid(frontMatter, "id", Path.GetFileNameWithoutExtension(filePath)),
                Name = GetValue(frontMatter, "name"),
                UrlHandle = GetValue(frontMatter, "urlHandle")
            };
        }

        private static string BuildMarkdown(Category category)
        {
            var frontMatter = new Dictionary<string, string>
            {
                ["id"] = category.Id.ToString(),
                ["name"] = category.Name,
                ["urlHandle"] = category.UrlHandle
            };

            return MarkdownFrontMatter.Build(frontMatter, string.Empty);
        }

        private string GetFilePath(Guid id)
        {
            return Path.Combine(categoriesDirectory, $"{id}.md");
        }

        private static string GetValue(IReadOnlyDictionary<string, string> frontMatter, string key)
        {
            return frontMatter.TryGetValue(key, out var value) ? value : string.Empty;
        }

        private static Guid ParseGuid(IReadOnlyDictionary<string, string> frontMatter, string key, string? fallbackId)
        {
            if (frontMatter.TryGetValue(key, out var value) && Guid.TryParse(value, out var parsed))
            {
                return parsed;
            }

            if (Guid.TryParse(fallbackId, out parsed))
            {
                return parsed;
            }

            return Guid.Empty;
        }
    }
}
