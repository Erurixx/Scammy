using Microsoft.AspNetCore.Mvc;
using Scammy.Data;
using Scammy.Models;

namespace Scammy.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArticleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var articles = _context.Articles.ToList();
            return Ok(articles);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Article article)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            article.Status = "published"; // if you want this default
            article.CreatedAt = DateTime.UtcNow;

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return Ok(article);
        }


        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Article updated)
        {
            var existing = await _context.Articles.FindAsync(updated.Id);
            if (existing == null) return NotFound();

            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return Ok();
        }

        
        

    }
}
