using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;

namespace Blog_Engine.Models.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public DateTime publicationDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        //public List<CategoryViewModel> Categories { get; set; }
        public SelectList Categories { get; set; }
}
}
