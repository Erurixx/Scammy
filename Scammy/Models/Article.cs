using System.ComponentModel.DataAnnotations;

namespace Scammy.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [StringLength(200)]
        public string Excerpt { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        public string ImagePath { get; set; }

        public string Tags { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "draft";

        public bool IsApproved { get; set; } = false;

        public string AdminComment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
