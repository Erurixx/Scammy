using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scammy.Data;
using Scammy.Models;
using System.Security.Claims;

namespace Scammy.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }


        



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage([Bind("Name,JobTitle,Email,CompanyName,Description")] ScamReport report)
        {
            if (ModelState.IsValid)
            {

                // Ensure status is set to Pending
                report.Status = "pending";

                _context.ScamReports.Add(report);
                await _context.SaveChangesAsync();

                // Simple alert notification
                TempData["AlertScript"] = "alert('Your scam report has been submitted successfully!');";
                return RedirectToAction("Index", "Home");
            }

            TempData["AlertScript"] = "alert('There was a problem submitting your report. Please try again.');";
            return RedirectToAction("Index", "Home");
        }


        

    }
}
