using Microsoft.AspNetCore.Mvc;

namespace Scammy.Controllers
{
    public class AnalystController : Controller
    {
        public IActionResult Index()
        {
            // This will handle the Manage Articles page
            return View(); // This will look for Views/Analyst/ManageArticles.cshtml
        }

        // Optional: Dashboard for Analyst
        public IActionResult dashboard()
        {
            return View(); // Looks for Views/Analyst/Dashboard.cshtml
        }

        public IActionResult createArticle()
        {
            ViewBag.ActivePage = "createArticle";
            return View(); // Looks for Views/Analyst/createArticle.cshtml
        }
    }
}
