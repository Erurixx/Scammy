using Microsoft.AspNetCore.Mvc;

namespace Scammy.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            return View("AdminDashboard");
        }

        public ActionResult ManageReport()
        {
            return View("ManageReport");
        }

        public ActionResult ManageUser()
        {
            return View("ManageUser");
        }

        public ActionResult CreateUser()
        {
            return View("CreateUser");
        }
    }
}
