namespace ReviewsWebApp.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageGuid { get; set; }
        public Review Review { get; set; }
        public int ReviewId { get; set; }
    }
}
