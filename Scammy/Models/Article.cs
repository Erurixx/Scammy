using System.ComponentModel.DataAnnotations;

namespace Scammy.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;

        public string Author { get; set; } // store analyst's username or ID

        public bool IsApproved { get; set; } = false;

        public string AdminComment { get; set; } // used for rejection reasons
    }
}
