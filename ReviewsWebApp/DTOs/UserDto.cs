using ReviewsWebApp.Models;

namespace ReviewsWebApp.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<ReviewDetailsDto> Reviews { get; set; }
        public IEnumerable<ReviewDetailsDto> LikedReviews { get; set; }
        public bool IsOwningAccount { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }
    }
}
