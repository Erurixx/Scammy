using System.ComponentModel.DataAnnotations;

namespace Scammy.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        public string Excerpt { get; set; }

        [Required]
        public string Content { get; set; }

        public string Tags { get; set; }

        public string Status { get; set; }

        public string ImagePath { get; set; }

        public string Author { get; set; }

        public bool IsApproved { get; set; } = false;

        public string AdminComment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
