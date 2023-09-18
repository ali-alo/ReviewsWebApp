using AutoMapper;
using ReviewsWebApp.ViewModels;

namespace ReviewsWebApp.Profiles
{
    public class ReviewViewModelProfile : Profile
    {
        public ReviewViewModelProfile()
        {
            CreateMap<ReviewFormViewModel, ReviewDetailsViewModel>();
        }
    }
}
