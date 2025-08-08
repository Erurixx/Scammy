using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scammy.Data;
using Scammy.Models;

namespace Scammy.Controllers
{
    public class AnalystController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AnalystController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult ManageArticles()
        {
            return View("~/Views/Analyst/ManageArticles.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles()
        {
            var articles = await _context.Articles.ToListAsync();
            return Json(articles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return Json(article);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArticle([FromBody] Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return Json(article);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }


}
