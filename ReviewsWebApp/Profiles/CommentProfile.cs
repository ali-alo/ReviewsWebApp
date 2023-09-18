using AutoMapper;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentFormDto, Comment>();
        }
    }
}
