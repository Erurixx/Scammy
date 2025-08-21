using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scammy.Data;
using Scammy.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Scammy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        //public HomeController(ILogger<HomeController> logger)

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> articles()
        {
            var publishedArticles = await _context.Articles
                .Where(a => a.Status == "published")
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(publishedArticles);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> viewAllArticles()
        {
            var publishedArticles = await _context.Articles
                .Where(a => a.Status == "published")
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(publishedArticles); // this will look for Views/Home/ViewAllArticles.cshtml
        }

        
        public async Task<IActionResult> readArticles(int id, string slug)
        {
            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.Id == id && a.Status == "published");

            if (article == null)
                return NotFound();

            // Fetch related articles (same category, exclude current article)
            var relatedArticles = await _context.Articles
                .Where(a => a.Category == article.Category && a.Id != id && a.Status == "published")
                .OrderByDescending(a => a.CreatedAt)
                .Take(5) // Limit to 5 related articles
                .ToListAsync();

            // Pass related articles to the view via ViewBag
            ViewBag.RelatedArticles = relatedArticles;


            return View(article); // Looks for Views/Articles/ReadArticle.cshtml
        }



    }
}
