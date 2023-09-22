using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.ViewModels
{
    public class ReviewsIndexViewModel
    {
        public ReviewsIndexViewModel(IEnumerable<ReviewDetailsDto> reviews, string containerLink, int pagesCount, 
            int currentPage, bool isFirstPage, bool isLastPage, string tagName)
        {
            Reviews = reviews;
            ContainerLink = containerLink;
            PagesCount = pagesCount;
            PageNumber = currentPage == 0 ? 1 : currentPage;
            IsFirstPage = isFirstPage;
            IsLastPage = isLastPage;
            TagName = tagName;
        }

        public IEnumerable<ReviewDetailsDto> Reviews { get; set; }
        public string ContainerLink { get; set; }
        public int PagesCount { get; set; }
        public int PageNumber { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set;}
        public string? TagName { get; set; }
    }
}
