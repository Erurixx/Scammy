using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scammy.Data;
using Scammy.Models;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Scammy.Controllers
{
    
    public class AnalystController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;


        public AnalystController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult createArticle()
        {
            ViewBag.ActivePage = "createArticle";
            return View();

        }

        //public IActionResult viewPublishedArticles()
        //{
        //    // optionally fetch data for the view
        //    return View();
        //}


        //Submit Form
        [HttpPost]
        public async Task<IActionResult> createArticle(Article model, IFormFile imageFile, string submitAction)
        {
   
            if (string.IsNullOrWhiteSpace(model.Title) ||
                string.IsNullOrWhiteSpace(model.Excerpt) ||
                string.IsNullOrWhiteSpace(model.Content) ||
                string.IsNullOrWhiteSpace(model.Category))
            {
                ModelState.AddModelError("", "Please fill in all required fields.");
                return View(model);
            }

            // Author
            model.Author = "Jasmine";

            // Status
            model.Status = submitAction == "saveDraft" ? "draft" : "pending";

            // Image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "articles");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                model.ImagePath = "/uploads/articles/" + uniqueFileName;
            }

            // CreatedAt
            model.CreatedAt = DateTime.UtcNow;

            // Optional tags from form
            var tagsInput = Request.Form["Tags"].ToString(); // raw input from form
            if (!string.IsNullOrWhiteSpace(tagsInput))
            {
                var tagsList = tagsInput.Split(',')
                                        .Select(t => t.Trim())
                                        .Where(t => !string.IsNullOrEmpty(t))
                                        .ToList();

                model.Tags = string.Join(",", tagsList);
            }
            else
            {
                model.Tags = "none"; // default value so it’s not null
            }


            // Save to DB
            _context.Articles.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = model.Status == "draft" ? "Draft saved." : "Article submitted.";

            return RedirectToAction("createArticle");
        }

        [HttpGet]
        public async Task<IActionResult> viewPublishedArticles()
        {
            ViewBag.ActivePage = "viewPublishedArticles";

            try
            {
                // Get all published articles ordered by creation date (newest first)
                var publishedArticles = await _context.Articles
    .Where(a => a.Status == "published")
    .ToListAsync();


                // Debugging: write count to console so you can see it in VS Output window
                Console.WriteLine($"✅ Published articles found: {publishedArticles.Count}");

                return View(publishedArticles);
            }
            catch (Exception ex)
            {
                // Log the error (you can implement proper logging)
                Console.WriteLine($"❌ Error retrieving published articles: {ex.Message}");

                // Return empty list in case of error
                return View(new List<Article>());
            
        }


        //try
        //{
        //    // Get all published articles ordered by creation date (newest first)
        //    var publishedArticles = await _context.Articles
        //        .Where(a => a.Status == "published")
        //        .OrderByDescending(a => a.CreatedAt)
        //        .ToListAsync();

        //    return View(publishedArticles);
        //}
        //catch (Exception ex)
        //{
        //    // Log the error (you can implement proper logging)
        //    Console.WriteLine($"Error retrieving published articles: {ex.Message}");

        //    // Return empty list in case of error
        //    return View(new List<Article>());
        //}
    }




    }
}
