using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scammy.Data;
using Scammy.Models;
using System.Linq;
using System.Security.Claims;

namespace Scammy.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Login page
        public IActionResult Login()
        {
            return View();
        }

        // GET: Submit Report page with user's reports
        [Authorize(Roles = "jobseeker")]
        public IActionResult submitReport()
        {
            var userEmail = User.FindFirstValue(System.Security.Claims.ClaimTypes.Email);

            // Get user's reports
            var userReports = _db.ScamReports
                .Where(r => r.Email == userEmail)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            // Set ViewBag data
            ViewBag.SubmittedReportsCount = userReports.Count;
            ViewBag.PendingReportsCount = userReports.Count(r => r.Status.ToLower() == "pending");
            ViewBag.ReviewedReportsCount = userReports.Count(r => r.Status.ToLower() != "pending");
            ViewBag.UserName = User.Identity.Name; // Use the name from claims

            return View(userReports);
        }


        // POST: Register new user
        [HttpPost]
        public IActionResult Register(string fullName, string email, string password)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "All fields are required.";
                return RedirectToAction("Login");
            }

            if (_db.Users.Any(u => u.Email == email))
            {
                TempData["Error"] = "Email is already registered.";
                return RedirectToAction("Login");
            }

            // Simple password encoding (replace with proper hashing)
            var hashedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            var user = new User
            {
                FullName = fullName,
                Email = email,
                Password = hashedPassword,
                UserRole = "jobseeker" // default role
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            TempData["Success"] = "Account created successfully. Please log in.";
            return RedirectToAction("Login");
        }

        // POST: Sign in user
        [HttpPost]
        public async Task<IActionResult> LoginUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Email and password are required.";
                return RedirectToAction("Login");
            }

            var hashedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            var user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == hashedPassword);

            if (user == null)
            {
                TempData["Error"] = "Invalid email or password.";
                return RedirectToAction("Login");
            }

            // ✅ Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName), // this becomes @User.Identity.Name
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole ?? "jobseeker")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // keeps user logged in
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            // ✅ Sign in the user
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "jobseeker")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var report = await _db.ScamReports
                .FirstOrDefaultAsync(r => r.Id == id && r.Email == userEmail);

            if (report != null)
            {
                _db.ScamReports.Remove(report);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("submitReport");
        }

    }
}
