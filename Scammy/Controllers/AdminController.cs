using Microsoft.AspNetCore.Mvc;
using Scammy.Data;
using Scammy.Models;
using System.Linq;

namespace Scammy.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public ActionResult Dashboard()
        {
            return View("AdminDashboard");
        }

        // Manage Article
        public IActionResult ManageArticle()
        {
            ViewBag.ActivePage = "ManageArticle"; // ⚠️ 这一行必须加
            var articles = _context.Articles.ToList(); // 或者你的 Model 数据
            return View(articles);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string action, string AdminComment)
        {
            var article = _context.Articles.Find(id);
            if (article == null) return NotFound();

            if (action == "approve")
            {
                article.Status = "published";
                article.IsApproved = true;
                article.AdminComment = "ok";
            }
            else if (action == "decline")
            {
                if (string.IsNullOrWhiteSpace(AdminComment))
                {
                    TempData["Error"] = "Comment is required when declining.";
                    return RedirectToAction("ManageArticle");
                }
                article.Status = "declined";
                article.IsApproved = false;
                article.AdminComment = AdminComment;
            }

            article.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("ManageArticle");
        }

        [HttpPost]
        public IActionResult ApproveArticle(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                article.IsApproved = true;
                article.Status = "published";
                _context.SaveChanges();
            }
            return RedirectToAction("ManageArticle");
        }

        [HttpPost]
        public IActionResult DeclineArticle(int id, string AdminComment)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                article.IsApproved = false;
                article.Status = "declined";
                article.AdminComment = AdminComment; // save the comment
                _context.SaveChanges();
            }
            return RedirectToAction("ManageArticle");
        }


        // Manage User
        public ActionResult ManageUser()
        {
            return View("ManageUser");
        }

        // Create User
        public ActionResult CreateUser()
        {
            return View("CreateUser");
        }
    }
}
