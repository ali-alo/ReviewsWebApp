namespace ReviewsWebApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Review> Reviews { get; set; }
    }
}
