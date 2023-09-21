namespace ReviewsWebApp.DTOs
{
    public class SearchDto
    {
        public string ObjectID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public SearchDto(int id, string title, string description)
        {
            ObjectID = id.ToString();
            Title = title;
            Description = description;
        }
    }
}
