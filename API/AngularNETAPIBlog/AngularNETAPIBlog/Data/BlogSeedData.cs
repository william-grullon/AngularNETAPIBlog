using AngularNETAPIBlog.Models.Domain;

namespace AngularNETAPIBlog.Data
{
    public static class BlogSeedData
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.AddRange(
                    new Category
                    {
                        Name = "Development",
                        UrlHandle = "development"
                    },
                    new Category
                    {
                        Name = "Learning Notes",
                        UrlHandle = "learning-notes"
                    });
            }

            if (!dbContext.BlogPosts.Any())
            {
                dbContext.BlogPosts.AddRange(
                    new BlogPost
                    {
                        Title = "Why I built this blog",
                        ShortDescription = "A quick note on turning a half-finished learning project into something shippable.",
                        Content = "This project started as a way to learn Angular, ASP.NET Core, Entity Framework, and repository patterns. Finishing it meant wiring the missing pieces together: a public home page, post details, and CRUD flows that actually work end to end.",
                        FeatureImageUrl = "https://images.unsplash.com/photo-1499750310107-5fef28a66643?auto=format&fit=crop&w=1200&q=80",
                        UrlHandle = "why-i-built-this-blog",
                        PublishedDate = new DateTime(2024, 2, 1, 9, 0, 0, DateTimeKind.Utc),
                        Author = "William Grullon",
                        IsVisible = true
                    },
                    new BlogPost
                    {
                        Title = "What the API now supports",
                        ShortDescription = "The API now handles blog posts, not just categories.",
                        Content = "The backend now exposes blog post create, read, update, delete endpoints in addition to categories. That gives the UI something real to work with and makes the project feel like a small CMS instead of a tutorial stub.",
                        FeatureImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=1200&q=80",
                        UrlHandle = "what-the-api-now-supports",
                        PublishedDate = new DateTime(2024, 2, 12, 10, 30, 0, DateTimeKind.Utc),
                        Author = "William Grullon",
                        IsVisible = true
                    },
                    new BlogPost
                    {
                        Title = "The next thing to polish",
                        ShortDescription = "A few natural follow-ups if you want to keep iterating.",
                        Content = "After the main flows are working, the next worthwhile improvements are category assignment for posts, validation, image uploads, and authentication for the admin area. Those would take this from a finished learning project to a more production-shaped app.",
                        FeatureImageUrl = "https://images.unsplash.com/photo-1516321497487-e288fb19713f?auto=format&fit=crop&w=1200&q=80",
                        UrlHandle = "the-next-thing-to-polish",
                        PublishedDate = new DateTime(2024, 2, 20, 14, 15, 0, DateTimeKind.Utc),
                        Author = "William Grullon",
                        IsVisible = true
                    });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
