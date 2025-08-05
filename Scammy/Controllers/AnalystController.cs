using Microsoft.AspNetCore.Mvc;

namespace Scammy.Controllers
{
    public class AnalystController : Controller
    {
        public IActionResult ManageArticles()
        {
            return View("~/Views/Analyst/ManageArticles.cshtml");
        }
    }
}
