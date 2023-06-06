using Blog_Engine.Data;
using Blog_Engine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Engine.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class categoriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public categoriesApiController(ApplicationDbContext applicationDbContext) => 
            _applicationDbContext = applicationDbContext;

        [HttpGet]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IEnumerable<Category>> Get() => 
            await _applicationDbContext.Categories.ToListAsync();


        [HttpGet("id")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            
            var category = await _applicationDbContext.Categories.FindAsync(id);
            return category == null ? NotFound() : Ok(category);
        }
        [HttpPost]
        [ProducesResponseType (StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(Category category)
        {
            await _applicationDbContext.Categories.AddAsync(category);
            await _applicationDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id) return BadRequest();

            _applicationDbContext.Entry(category).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("{id}/posts")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPosts(int id)
        {

            var posts = await _applicationDbContext.Post.Where(x => x.CategoryId == id).ToListAsync();
            return posts == null ? NotFound() : Ok(posts);
        }
    }

    
}
