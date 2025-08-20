using Microsoft.AspNetCore.Mvc;
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
                ViewBag.Error = "All fields are required.";
                return View("Login");
            }

            if (_db.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Email is already registered.";
                return View("Login");
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

            ViewBag.Success = "Account created successfully. Please log in.";
            return View("Login");
        }

        // POST: Sign in user
        [HttpPost]
        public IActionResult LoginUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View("Login");
            }

            var hashedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            var user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == hashedPassword);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View("Login");
            }

            // Optionally: store user info in TempData/session
            TempData["UserName"] = user.FullName;
            TempData["UserRole"] = user.UserRole;

            return RedirectToAction("Index", "Home");
        }
    }
}
