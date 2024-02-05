using AngularNETAPIBlog.API.Models.DTO;
using AngularNETAPIBlog.API.Repositories.Interface;
using AngularNETAPIBlog.Data;
using AngularNETAPIBlog.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AngularNETAPIBlog.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository) 
        {
            this.categoryRepository = categoryRepository;
        }

        // POST: http://localhost:5201/api/Categories from the request body
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            // Map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateCategoryAsync(category);

            // Domain Model to DTO
            var response = new CategoryDTO
            { 
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        // GET: http://localhost:5201/api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllCategoriesAsync();

            // Domain Model to DTO
            var response = categories.Select(category => new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            });

            // alternative Domain Model to DTO foreach loop
            // var response = new List<CategoryDTO>();
            // foreach (var category in categories)
            // {
            //     response.Add(new CategoryDTO
            //     {                
            //         Id = category.Id,                
            //         Name = category.Name,                
            //         UrlHandle = category.UrlHandle
            //     });
            // }                    

            return Ok(response);
        }

        // GET: http://localhost:5201/api/Categories/{id} from the route
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            // Domain Model to DTO
            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
            
        }
    }
}
