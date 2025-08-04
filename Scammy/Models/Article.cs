using System.ComponentModel.DataAnnotations;

namespace Scammy.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
