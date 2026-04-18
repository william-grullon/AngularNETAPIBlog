using AngularNETAPIBlog.API.Models.DTO;
using AngularNETAPIBlog.API.Repositories.Interface;
using AngularNETAPIBlog.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AngularNETAPIBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeatureImageUrl = request.FeatureImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate == default ? DateTime.UtcNow : request.PublishedDate,
                Author = request.Author,
                IsVisible = request.IsVisible
            };

            await blogPostRepository.CreateBlogPostAsync(blogPost);

            return Ok(MapToDto(blogPost));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllBlogPostsAsync();
            var response = blogPosts.Select(MapToDto);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.GetBlogPostByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(MapToDto(blogPost));
        }

        [HttpGet("by-url/{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await blogPostRepository.GetBlogPostByUrlHandleAsync(urlHandle);
            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(MapToDto(blogPost));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, [FromBody] UpdateBlogPostRequestDto request)
        {
            var blogPost = await blogPostRepository.GetBlogPostByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            blogPost.Title = request.Title;
            blogPost.ShortDescription = request.ShortDescription;
            blogPost.Content = request.Content;
            blogPost.FeatureImageUrl = request.FeatureImageUrl;
            blogPost.UrlHandle = request.UrlHandle;
            blogPost.PublishedDate = request.PublishedDate == default ? blogPost.PublishedDate : request.PublishedDate;
            blogPost.Author = request.Author;
            blogPost.IsVisible = request.IsVisible;

            await blogPostRepository.UpdateBlogPostAsync(blogPost);
            return Ok(MapToDto(blogPost));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.GetBlogPostByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            await blogPostRepository.DeleteBlogPostAsync(blogPost);
            return Ok(MapToDto(blogPost));
        }

        private static BlogPostDTO MapToDto(BlogPost blogPost)
        {
            return new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible
            };
        }
    }
}
