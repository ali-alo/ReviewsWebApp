namespace ReviewsWebApp.DTOs
{
    public class UserListDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ReviewsCount { get; set; }
        public decimal ReviewsAverageRating { get; set; }
        public int LikesAmount { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }
    }
}
