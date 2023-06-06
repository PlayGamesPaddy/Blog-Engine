using System.ComponentModel.DataAnnotations;

namespace Blog_Engine.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public DateTime publicationDate { get; set; }
        public string Content { get; set; }
    }
}
