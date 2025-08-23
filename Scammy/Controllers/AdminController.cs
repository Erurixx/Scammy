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




        // Manage User page
        public IActionResult ManageUser()
        {
            
            var admins = _context.Users.Where(u => u.UserRole == "Admin").ToList();
            var analysts = _context.Users.Where(u => u.UserRole == "Analyst").ToList();
            var jobseekers = _context.Users.Where(u => u.UserRole == "Jobseeker").ToList(); // 改成 Jobseeker

            ViewBag.Admins = admins;
            ViewBag.Analysts = analysts;
            ViewBag.Jobseekers = jobseekers; // 对应前端 Tab

            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult DeactivateUser([FromBody] UserToggleDto data)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == data.Id);
            if (user == null) return NotFound();

            user.IsActive = false;
            _context.SaveChanges();

            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult ActivateUser([FromBody] UserToggleDto data)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == data.Id);
            if (user == null) return NotFound();

            user.IsActive = true;
            _context.SaveChanges();

            return Ok(new { success = true });
        }

        public class UserToggleDto
        {
            public int Id { get; set; }
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
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { isSuccess = true, message = "User created successfully!" });
        }



        // Manage Report
        public IActionResult ManageReport()
        {
            ViewBag.ActivePage = "ManageReport";

            // Fetch all reports from DB, sorted by CreatedAt descending
            var reports = _context.ScamReports
                                  .OrderByDescending(r => r.CreatedAt)
                                  .ToList();

            return View(reports);
        }

        // Update status of a report: Approve or Reject
        [HttpPost]
        public IActionResult UpdateReportStatus(int id, string action)
        {
            var report = _context.ScamReports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();

            // Update status based on action
            switch (action.ToLower())
            {
                case "approve":
                    report.Status = "Approved";
                    break;
                case "reject":
                    report.Status = "Rejected";
                    break;
                case "pending":
                    report.Status = "Pending";
                    break;
                default:
                    return BadRequest("Invalid action");
            }

            _context.SaveChanges();
            return RedirectToAction("ManageReport");
        }





    }


}

