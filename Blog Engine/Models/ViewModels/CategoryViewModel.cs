using System.ComponentModel.DataAnnotations;
namespace Blog_Engine.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
