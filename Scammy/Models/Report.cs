using System;
using System.ComponentModel.DataAnnotations;

namespace Scammy.Models
{
    public class ScamReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }   // Reporter’s Name

        [Required]
        [StringLength(150)]
        public string JobTitle { get; set; }   // Job Title or Scam Name

        [Required]
        [EmailAddress]
        public string Email { get; set; }   // Reporter’s Email

        [StringLength(150)]
        public string CompanyName { get; set; }   // Company Name (if known)

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }   // Detailed Scam Description

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pending";   // Status: Pending, Approved, Rejected

        public DateTime CreatedAt { get; set; } = DateTime.Now;   // Auto timestamp
    }
}
