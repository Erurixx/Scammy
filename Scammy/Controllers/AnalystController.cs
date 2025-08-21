using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scammy.Data;
using Scammy.Models;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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

            // Set success message based on status
            if (model.Status == "draft")
            {
                TempData["SuccessMessage"] = "Draft saved successfully.";
            }
            else if (model.Status == "pending")
            {
                TempData["SuccessMessage"] = "Article submitted. Please wait for admin approval.";
            }

            await _context.SaveChangesAsync();

            //TempData["SuccessMessage"] = model.Status == "draft" ? "Draft saved." : "Article submitted.";

            return RedirectToAction("createArticle");
        }

        [HttpGet]
        public async Task<IActionResult> viewPublishedArticles()
        {
            ViewBag.ActivePage = "viewPublishedArticles";

            

            var publishedArticles = await _context.Articles
       .Where(a => a.Status != null && a.Status.Trim().ToLower() == "published")
       .OrderByDescending(a => a.CreatedAt)
       .ToListAsync();

            var pendingArticles = await _context.Articles
                .Where(a => a.Status != null && a.Status.Trim().ToLower() == "pending")
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            var draftArticles = await _context.Articles
                .Where(a => a.Status != null && a.Status.Trim().ToLower() == "draft")
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            // Combine all articles into one list for the view
            var allArticles = publishedArticles.Concat(pendingArticles).Concat(draftArticles).ToList();

            // Pass them to ViewBag for stats
            ViewBag.PublishedArticles = publishedArticles;
            ViewBag.PendingArticles = pendingArticles;
            ViewBag.DraftArticles = draftArticles;

            return View(allArticles); // Model will contain all articles



        }

        [HttpPost]
        public async Task<IActionResult> EditArticle(int id, Article model, IFormFile imageFile, string submitAction)
        {
            if (string.IsNullOrWhiteSpace(model.Title) ||
                string.IsNullOrWhiteSpace(model.Excerpt) ||
                string.IsNullOrWhiteSpace(model.Content) ||
                string.IsNullOrWhiteSpace(model.Category))
            {
                ModelState.AddModelError("", "Please fill in all required fields.");
                return View("createArticle", model);
            }

            var existingArticle = await _context.Articles.FindAsync(id);
            if (existingArticle == null)
                return NotFound();

            // Update fields
            existingArticle.Title = model.Title;
            existingArticle.Excerpt = model.Excerpt;
            existingArticle.Content = model.Content;
            existingArticle.Category = model.Category;
            existingArticle.Status = submitAction == "saveDraft" ? "draft" : "pending";

            // Optional tags
            var tagsInput = Request.Form["Tags"].ToString();
            existingArticle.Tags = !string.IsNullOrWhiteSpace(tagsInput) ?
                                    string.Join(",", tagsInput.Split(',').Select(t => t.Trim())) : "none";

            // Image upload if new image is provided
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "articles");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                existingArticle.ImagePath = "/uploads/articles/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = existingArticle.Status == "draft" ? "Draft saved." : "Article updated.";
            return RedirectToAction("viewPublishedArticles");
        }

        [HttpGet]
        public async Task<IActionResult> EditArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            ViewBag.ActivePage = "createArticle"; // optional: highlights nav
            return View("createArticle", article); // reuse the createArticle view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return RedirectToAction("viewPublishedArticles");
        }


        


















    }
}
