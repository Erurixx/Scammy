using Microsoft.AspNetCore.Mvc;
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


//Submit Form
        [HttpPost]
        public async Task<IActionResult> createArticle(Article model, IFormFile imageFile, string submitAction)
        {
            //Console.WriteLine("POST received");
            //Console.WriteLine($"Title: {model.Title}");
            //Console.WriteLine($"Status: {submitAction}");
            //model.Author = User.Identity?.Name ?? "JasmineTest"; // <-- put this line here
            //model.CreatedAt = DateTime.UtcNow;
            //model.ImagePath = "/uploads/articles/2f1e4041-7856-45ac-8705-b34f4ccbaa13.png";
            //model.Tags = "phishing";

            //_context.Articles.Add(model);
            //await _context.SaveChangesAsync();
            //Console.WriteLine("Saved to database");

            //return RedirectToAction("createArticle");

            // Server-side validation
            // Validation
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





    }
}
