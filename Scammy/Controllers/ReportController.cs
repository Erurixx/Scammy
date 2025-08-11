using Microsoft.AspNetCore.Mvc;
using Scammy.Data;
using Scammy.Models;
using Scammy.ViewModels;
using System;
using System.Linq;

namespace Scammy.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Submit()
        {
            var model = new ScamReportViewModel
            {
                ScamDate = DateTime.Today // default date
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(ScamReportViewModel model)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Jobseeker"))
            {
                ModelState.AddModelError("", "You must be logged in as a Jobseeker to submit a scam report.");
            }

            if (model.SelectedScamTypes == null || !model.SelectedScamTypes.Any())
            {
                ModelState.AddModelError(nameof(model.SelectedScamTypes), "Please select at least one scam type.");
            }

            if (!ModelState.IsValid)
            {
                // reload options if invalid
                model.ScamTypeOptions = new ScamReportViewModel().ScamTypeOptions;
                return View(model);
            }

            var scamReport = new ScamReport
            {
                ScamTitle = model.ScamTitle,
                CompanyName = model.CompanyName,
                JobTitle = model.JobTitle,
                Salary = model.Salary,
                Description = model.Description,
                ContactEmail = model.ContactEmail,
                ScamTypes = string.Join(",", model.SelectedScamTypes),
                ScamDate = model.ScamDate,
                ReportedDate = DateTime.Now,
                ReportedByUserName = User.Identity.Name
            };

            _context.ScamReports.Add(scamReport);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Scam report submitted successfully.";
            return RedirectToAction("Submit");
        }
    }
}
