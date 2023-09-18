using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsWebApp.DTOs;
using ReviewsWebApp.Models;
using ReviewsWebApp.Repositories.Interfaces;
using System.Security.Claims;

namespace ReviewsWebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRatedReviewRepository _userRatedReviewRepository;

        public CommentController(IMapper mapper, ICommentRepository commentRepository,
            IReviewRepository reviewRepository, IUserRatedReviewRepository userRatedReviewRepository)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
            _reviewRepository = reviewRepository;
            _userRatedReviewRepository = userRatedReviewRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CommentFormDto commentDto)
        {
            ModelState.Clear();
            commentDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (TryValidateModel(commentDto)) // comment content passed the validation; user added a comment
            {
                var comment = _mapper.Map<Comment>(commentDto);
                await _commentRepository.CreateComment(comment);
            }
            await RateReview(commentDto);
            return RedirectToAction("Details", "Reviews", new { id = commentDto.ReviewId });
        }

        private async Task RateReview(CommentFormDto commentDto)
        {
            if (commentDto.Grade != 0)  //user rates the review
            {
                //true if user rated this review before
                bool reviewRatingExisted = await _userRatedReviewRepository.UpdateReviewRatingIfExists(commentDto.ReviewId, commentDto.UserId, commentDto.Grade);
                if (!reviewRatingExisted)
                {
                    var userRatedReview = new UserRatedReview
                    {
                        Rating = commentDto.Grade,
                        ReviewId = commentDto.ReviewId,
                        UserId = commentDto.UserId
                    };
                    await _userRatedReviewRepository.AddRatingToReview(userRatedReview);
                }
            }
        }
    }
}
