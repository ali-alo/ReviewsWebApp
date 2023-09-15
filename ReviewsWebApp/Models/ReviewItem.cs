using System.ComponentModel.DataAnnotations;

namespace ReviewsWebApp.Models
{
    public class ReviewItem
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string NameEn { get; set; }
        [MaxLength(100)]
        public string NameRu { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionRu { get; set; }
        [MaxLength(100)]
        public string ImageGuid { get; set; }
        public List<Review> Reviews { get; set; }
        public ReviewGroup ReviewGroup { get; set; }
        public int ReviewGroupId { get; set; }
    }
}
