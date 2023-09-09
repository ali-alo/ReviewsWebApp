using System.ComponentModel.DataAnnotations;
using ReviewsWebApp.Resources.Models;
namespace ReviewsWebApp.DTOs
{
    public class ReviewItemCreateDto
    {
        [Display(ResourceType = typeof(ReviewItemResource), Name = "Name")]
        [MaxLength(50, ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "NameTooLong")]
        [Required(ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(ReviewItemResource), Name = "Image")]
        [Required(ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "ImageRequired")]
        public IFormFile ImageFile { get; set; }
        [Display(ResourceType = typeof(ReviewItemResource), Name = "Description")]
        [Required(ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "DescriptionRequired")]
        public string Description { get; set; }
    }
}
