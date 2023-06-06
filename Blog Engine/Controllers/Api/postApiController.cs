using Blog_Engine.Data;
using Blog_Engine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Engine.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class postApiController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public postApiController(ApplicationDbContext applicationDbContext) =>
            _applicationDbContext = applicationDbContext;
        [HttpGet]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IEnumerable<Post>> Get() =>
            await _applicationDbContext.Post.Where(x => x.publicationDate < DateTime.Now).OrderByDescending(x => x.publicationDate).ToListAsync();
        [HttpGet("id")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _applicationDbContext.Post.FindAsync(id);
            return post == null ? NotFound() : Ok(post);
        }
        [HttpPost]
        [ProducesResponseType (StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(Post post)
        {
            await _applicationDbContext.Post.AddAsync(post);
            await _applicationDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if(id != post.Id) return BadRequest();

            _applicationDbContext.Entry(post).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
