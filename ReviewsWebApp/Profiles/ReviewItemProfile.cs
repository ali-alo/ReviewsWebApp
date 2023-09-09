using AutoMapper;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Profiles
{
    public class ReviewItemProfile : Profile
    {
        public ReviewItemProfile()
        {
            CreateMap<ReviewItem, ReviewItemDto>();
            CreateMap<ReviewItemCreateDto, ReviewItem>();
            CreateMap<ReviewItemDto, ReviewItem>();
        }
    }
}
