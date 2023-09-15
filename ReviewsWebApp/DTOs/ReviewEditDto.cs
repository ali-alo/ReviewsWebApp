using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReviewsWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.DTOs
{
    public class ReviewEditDto
    {
        public int Id { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ReviewResource))]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Models.ReviewResource), ErrorMessageResourceName = "TitlePropertyLong")]
        public string Title { get; set; } = string.Empty;
        public string MarkdownText { get; set; } = string.Empty;
        [Range(0, 10)]
        public decimal Grade { get; set; }
        public string? CreatorId { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        [BindNever]
        public string ReviewItemTitleEn { get; set; }
        [BindNever]
        public string ReviewItemTitleRu { get; set; }
        public int CommentsAmount { get; set; }
        public int LikesAmount { get; set; }
    }
}
