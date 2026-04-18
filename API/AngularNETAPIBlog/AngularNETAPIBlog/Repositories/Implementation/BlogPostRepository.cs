using AngularNETAPIBlog.API.Repositories.Interface;
using AngularNETAPIBlog.Models.Domain;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AngularNETAPIBlog.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly string postsDirectory;

        public BlogPostRepository(IWebHostEnvironment environment)
        {
            postsDirectory = Path.Combine(environment.ContentRootPath, "content", "posts");
            Directory.CreateDirectory(postsDirectory);
        }

        public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost)
        {
            if (blogPost.Id == Guid.Empty)
            {
                blogPost.Id = Guid.NewGuid();
            }

            await SaveBlogPostAsync(blogPost);
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            var blogPosts = new List<BlogPost>();

            foreach (var filePath in Directory.EnumerateFiles(postsDirectory, "*.md"))
            {
                blogPosts.Add(await ReadBlogPostAsync(filePath));
            }

            return blogPosts
                .OrderByDescending(x => x.PublishedDate)
                .ThenByDescending(x => x.Title);
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(Guid id)
        {
            var filePath = await FindFilePathByIdAsync(id);
            if (filePath is null)
            {
                return null;
            }

            return await ReadBlogPostAsync(filePath);
        }

        public async Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle)
        {
            foreach (var filePath in Directory.EnumerateFiles(postsDirectory, "*.md"))
            {
                var blogPost = await ReadBlogPostAsync(filePath);
                if (string.Equals(blogPost.UrlHandle, urlHandle, StringComparison.OrdinalIgnoreCase))
                {
                    return blogPost;
                }
            }

            return null;
        }

        public async Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            await SaveBlogPostAsync(blogPost);
        }

        public async Task DeleteBlogPostAsync(BlogPost blogPost)
        {
            var filePath = await FindFilePathByIdAsync(blogPost.Id);
            if (filePath is not null && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private async Task SaveBlogPostAsync(BlogPost blogPost)
        {
            var existingFilePath = await FindFilePathByIdAsync(blogPost.Id);
            if (existingFilePath is not null && File.Exists(existingFilePath))
            {
                File.Delete(existingFilePath);
            }

            await File.WriteAllTextAsync(GetFilePath(blogPost), BuildMarkdown(blogPost));
        }

        private async Task<BlogPost> ReadBlogPostAsync(string filePath)
        {
            var markdown = await File.ReadAllTextAsync(filePath);
            return ParseBlogPost(markdown, Path.GetFileNameWithoutExtension(filePath));
        }

        private async Task<string?> FindFilePathByIdAsync(Guid id)
        {
            foreach (var filePath in Directory.EnumerateFiles(postsDirectory, "*.md"))
            {
                var markdown = await File.ReadAllTextAsync(filePath);
                var (frontMatter, _) = MarkdownFrontMatter.Parse(markdown);
                if (ParseGuid(frontMatter, "id", null) == id)
                {
                    return filePath;
                }
            }

            return null;
        }

        private static BlogPost ParseBlogPost(string markdown, string? fallbackId = null)
        {
            var (frontMatter, body) = MarkdownFrontMatter.Parse(markdown);

            return new BlogPost
            {
                Id = ParseGuid(frontMatter, "id", fallbackId),
                Title = GetValue(frontMatter, "title"),
                ShortDescription = GetValue(frontMatter, "shortDescription"),
                Content = body,
                FeatureImageUrl = GetValue(frontMatter, "featureImageUrl"),
                UrlHandle = GetValue(frontMatter, "urlHandle"),
                PublishedDate = ParseDate(frontMatter, "publishedDate"),
                Author = GetValue(frontMatter, "author"),
                IsVisible = ParseBool(frontMatter, "isVisible")
            };
        }

        private string GetFilePath(BlogPost blogPost)
        {
            var timestamp = blogPost.PublishedDate.ToUniversalTime().ToString("yyyy-MM-dd-HHmmss", CultureInfo.InvariantCulture);
            var slug = Slugify(blogPost.UrlHandle);
            return Path.Combine(postsDirectory, $"{timestamp}--{slug}--{blogPost.Id}.md");
        }

        private static string BuildMarkdown(BlogPost blogPost)
        {
            var frontMatter = new Dictionary<string, string>
            {
                ["id"] = blogPost.Id.ToString(),
                ["title"] = blogPost.Title,
                ["shortDescription"] = blogPost.ShortDescription,
                ["featureImageUrl"] = blogPost.FeatureImageUrl,
                ["urlHandle"] = blogPost.UrlHandle,
                ["publishedDate"] = blogPost.PublishedDate.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture),
                ["author"] = blogPost.Author,
                ["isVisible"] = blogPost.IsVisible.ToString().ToLowerInvariant()
            };

            return MarkdownFrontMatter.Build(frontMatter, blogPost.Content);
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

        private static DateTime ParseDate(IReadOnlyDictionary<string, string> frontMatter, string key)
        {
            if (frontMatter.TryGetValue(key, out var value) &&
                DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed))
            {
                return parsed;
            }

            return DateTime.UtcNow;
        }

        private static bool ParseBool(IReadOnlyDictionary<string, string> frontMatter, string key)
        {
            if (frontMatter.TryGetValue(key, out var value) && bool.TryParse(value, out var parsed))
            {
                return parsed;
            }

            return false;
        }

        private static string Slugify(string value)
        {
            var slug = value.Trim().ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9]+", "-");
            slug = Regex.Replace(slug, @"-+", "-").Trim('-');
            return string.IsNullOrWhiteSpace(slug) ? "post" : slug;
        }
    }
}
