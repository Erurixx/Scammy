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
            ViewBag.ActivePage = "ManageArticle"; 
            var articles = _context.Articles.ToList(); 
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
                article.AdminComment = AdminComment;
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
        public IActionResult ApproveArticle(int id, string AdminComment)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                article.IsApproved = true;
                article.Status = "published";
                article.AdminComment = AdminComment; // Save the admin comment
                article.UpdatedAt = DateTime.Now;
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
            ViewBag.ActivePage = "ManageUser";
            return View("ManageUser");
        }


        // Create User
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(string fullName, string email, string password, string role)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                return BadRequest("All fields are required.");
            }

            role = role.ToLower();
            if (role != "admin" && role != "analyst")
            {
                return BadRequest("Only admin or analyst roles can be assigned.");
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                return BadRequest("Email is already registered.");
            }

            var hashedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            var user = new User
            {
                FullName = fullName,
                Email = email,
                Password = hashedPassword,
                UserRole = role,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { isSuccess = true, message = "User created successfully!" });
        }









    }


}

