using System;

namespace Scammy.Models
{
    public class ScamReport
    {
        public int Id { get; set; }
        public string ScamTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;

        // Store multiple scam types as comma separated string
        public string ScamTypes { get; set; } = string.Empty;

        public DateTime ScamDate { get; set; }
        public DateTime ReportedDate { get; set; }
        public string ReportedByUserName { get; set; } = string.Empty;
    }
}
