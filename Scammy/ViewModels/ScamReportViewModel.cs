using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Scammy.ViewModels
{
    public class ScamReportViewModel
    {
        [Required]
        [Display(Name = "Scam Title")]
        public string ScamTitle { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select at least one scam type.")]
        [Display(Name = "Scam Types")]
        public List<string> SelectedScamTypes { get; set; } = new List<string>();

        // This list is for rendering options in the View
        public List<ScamTypeOption> ScamTypeOptions { get; set; } = new List<ScamTypeOption>()
        {
            new ScamTypeOption{ Id="Phishing", Name="Phishing" },
            new ScamTypeOption{ Id="FakeJob", Name="Fake Job" },
            new ScamTypeOption{ Id="Overpayment", Name="Overpayment" },
            new ScamTypeOption{ Id="IdentityTheft", Name="Identity Theft" },
            new ScamTypeOption{ Id="Others", Name="Others" }
        };

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Scam Date")]
        public DateTime ScamDate { get; set; }
    }

    public class ScamTypeOption
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
