using System.ComponentModel.DataAnnotations;
using ReviewsWebApp.Resources.Models;
namespace ReviewsWebApp.DTOs
{
    public class ReviewItemCreateDto
    {
        [Display(ResourceType = typeof(ReviewItemResource), Name = "NameEn")]
        [MaxLength(50, ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "NameTooLong")]
        [Required(ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "NameRequired")]
        public string NameEn { get; set; }
        [Display(ResourceType = typeof(ReviewItemResource), Name = "NameRu")]
        [MaxLength(50, ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "NameTooLong")]
        [Required(ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "NameRequired")]
        public string NameRu { get; set; }
        [Display(ResourceType = typeof(ReviewItemResource), Name = "Image")]
        [Required(ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "ImageRequired")]
        public IFormFile ImageFile { get; set; }
        [Display(ResourceType = typeof(ReviewItemResource), Name = "DescriptionEn")]
        [Required(ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "DescriptionRequired")]
        public string DescriptionEn { get; set; }
        [Display(ResourceType = typeof(ReviewItemResource), Name = "DescriptionRu")]
        [Required(ErrorMessageResourceType = typeof(ReviewItemResource), ErrorMessageResourceName = "DescriptionRequired")]
        public string DescriptionRu { get; set; }
        public int ReviewGroupId { get; set; }
    }
}
