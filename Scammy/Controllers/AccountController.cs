using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Scammy.Data;
using Scammy.Models;
using System.Linq;

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
                UserRole = "jobseeker",
                IsActive = true
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

            // ✅ check IsActive
            if (!user.IsActive)
            {
                TempData["Error"] = "Your account has been deactivated. Please contact administrator.";
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
    }
}
