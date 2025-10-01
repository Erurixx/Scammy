using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scammy.Data;
using Scammy.Models;
using Scammy.Services;  // ADD THIS - For FileUploadService
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Scammy.Controllers
{
    public class AnalystController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly FileUploadService _fileUploadService;  // ADD THIS
        private readonly ILogger<AnalystController> _logger;    // ADD THIS

        // UPDATED Constructor
        public AnalystController(
            ApplicationDbContext context,
            IWebHostEnvironment env,
            FileUploadService fileUploadService,        // ADD THIS
            ILogger<AnalystController> logger)           // ADD THIS
        {
            _context = context;
            _env = env;
            _fileUploadService = fileUploadService;      // ADD THIS
            _logger = logger;                            // ADD THIS
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> dashboard()
        {
            string username = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.FullName == username);
            string analystName = currentUser != null ? currentUser.FullName : "Unknown";

            // Article status counts
            var totalArticles = await _context.Articles.CountAsync();
            var approvedArticles = await _context.Articles.CountAsync(a => a.Status.ToLower() == "published");
            var pendingArticles = await _context.Articles.CountAsync(a => a.Status.ToLower() == "pending");
            var draftArticles = await _context.Articles.CountAsync(a => a.Status.ToLower() == "draft");

            // Daily articles for chart (last 30 days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30).Date;
            var chartData = await _context.Articles
                .Where(a => a.CreatedAt.Date >= thirtyDaysAgo && a.Author == analystName)
                .GroupBy(a => a.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(g => g.Date)
                .ToListAsync();

            // Chart data for the small chart (total articles created over time)
            var totalArticlesChartData = await _context.Articles
                .Where(a => a.Author == analystName)
                .GroupBy(a => a.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(g => g.Date)
                .Take(10)
                .ToListAsync();

            // Approval rate data for small charts
            var approvalRate = totalArticles > 0 ? (double)approvedArticles / totalArticles * 100 : 0;

            ViewBag.TotalArticles = totalArticles;
            ViewBag.ApprovedArticles = approvedArticles;
            ViewBag.PendingArticles = pendingArticles;
            ViewBag.DraftArticles = draftArticles;
            ViewBag.ChartData = chartData;
            ViewBag.TotalArticlesChartData = totalArticlesChartData;
            ViewBag.ApprovalRate = Math.Round(approvalRate, 2);

            return View();
        }

        [HttpGet]
        public IActionResult createArticle()
        {
            var model = new Article();

            // Populate Author for logged-in user
            string username = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.FullName == username);
            model.Author = currentUser != null ? currentUser.FullName : "Unknown";

            return View(model);
        }

        // UPDATED Submit Form - NOW USES SERVERLESS MICROSERVICE
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

            // Author (based on logged-in user)
            string username = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.FullName == username);
            model.Author = currentUser != null ? currentUser.FullName : "Unknown";

            // Status
            model.Status = submitAction == "saveDraft" ? "draft" : "pending";

            // UPDATED: Image upload via Lambda microservice to S3
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    _logger.LogInformation($"Uploading image via serverless microservice: {imageFile.FileName}");

                    // Upload to S3 via Lambda (serverless microservice)
                    var uploadResult = await _fileUploadService.UploadFileAsync(imageFile);

                    // Store S3 URL and key
                    model.ImagePath = uploadResult.FileUrl;
                    model.ImageS3Key = uploadResult.S3Key;

                    _logger.LogInformation($"Image uploaded successfully to S3: {uploadResult.FileUrl}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading image to S3 via Lambda");
                    TempData["ErrorMessage"] = "Failed to upload image. Please try again.";
                    return View(model);
                }
            }

            // CreatedAt
            model.CreatedAt = DateTime.UtcNow;
            model.AdminComment = "N/A";

            // Optional tags from form
            var tagsInput = Request.Form["Tags"].ToString();
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
                model.Tags = "none";
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

            return RedirectToAction("createArticle");
        }

        [HttpGet]
        public async Task<IActionResult> viewPublishedArticles()
        {
            ViewBag.ActivePage = "viewPublishedArticles";

            string username = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.FullName == username);
            string analystName = currentUser != null ? currentUser.FullName : "Unknown";

            var publishedArticles = await _context.Articles
                .Where(a => a.Status != null && a.Status.Trim().ToLower() == "published" && a.Author == analystName)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            var pendingArticles = await _context.Articles
                .Where(a => a.Status != null && a.Status.Trim().ToLower() == "pending" && a.Author == analystName)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            var draftArticles = await _context.Articles
                .Where(a => a.Status != null && a.Status.Trim().ToLower() == "draft" && a.Author == analystName)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            var declinedArticles = await _context.Articles
                .Where(a => a.Status != null && a.Status.Trim().ToLower() == "declined" && a.Author == analystName)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            // Combine all articles into one list for the view
            var allArticles = publishedArticles.Concat(pendingArticles).Concat(draftArticles).Concat(declinedArticles).ToList();

            // Pass them to ViewBag for stats
            ViewBag.PublishedArticles = publishedArticles;
            ViewBag.PendingArticles = pendingArticles;
            ViewBag.DraftArticles = draftArticles;
            ViewBag.DeclinedArticles = declinedArticles;

            return View(allArticles);
        }

        // UPDATED EditArticle - NOW USES SERVERLESS MICROSERVICE
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

            string username = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.FullName == username);
            string analystName = currentUser != null ? currentUser.FullName : "Unknown";

            if (existingArticle.Author != analystName)
                return Forbid();

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

            // UPDATED: Image upload via Lambda microservice to S3 if new image is provided
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    _logger.LogInformation($"Uploading new image via serverless microservice: {imageFile.FileName}");

                    // Upload to S3 via Lambda (serverless microservice)
                    var uploadResult = await _fileUploadService.UploadFileAsync(imageFile);

                    // Update with new S3 URL and key
                    existingArticle.ImagePath = uploadResult.FileUrl;
                    existingArticle.ImageS3Key = uploadResult.S3Key;

                    _logger.LogInformation($"New image uploaded successfully to S3: {uploadResult.FileUrl}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading new image to S3 via Lambda");
                    TempData["ErrorMessage"] = "Failed to upload new image.";
                    return View("createArticle", existingArticle);
                }
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

            string username = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.FullName == username);
            string analystName = currentUser != null ? currentUser.FullName : "Unknown";

            if (article.Author != analystName)
                return Forbid();

            ViewBag.ActivePage = "createArticle";
            return View("createArticle", article);
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

            string username = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.FullName == username);
            string analystName = currentUser != null ? currentUser.FullName : "Unknown";

            if (article.Author != analystName)
                return Forbid();

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return RedirectToAction("viewPublishedArticles");
        }
    }
}