using ReviewsWebApp.DTOs;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewItemsListViewModel
    {
        public List<ReviewItemDto> Items { get; set; }
        public string ContainerLink { get; set; }
    }
}
