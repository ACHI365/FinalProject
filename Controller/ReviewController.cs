using System.Security.Claims;
using FinalProject.Data;
using FinalProject.Model;
using FinalProject.Model.Dto;
using FinalProject.Model.Relations;
using FinalProject.Service;
using FinalProject.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controller;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly DataContext _dbContext;
    private readonly IReviewService _reviewService;

    public ReviewController(DataContext dbContext, IReviewService reviewService)
    {
        _reviewService = reviewService;
        _dbContext = dbContext;
    }

    [HttpPost("create-review")]
    // [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto model)
    {
        var user = await _dbContext.Users.FindAsync(model.UserId);
        var piece = await _dbContext.Pieces.FindAsync(model.PieceId);

        Console.WriteLine(model.UserId);
        Console.WriteLine(model.PieceId);
        
        if (user == null || piece == null)
            return BadRequest("Invalid user or piece.");
    
        var review = new Review
        {
            ReviewName = model.ReviewName,
            Group = model.Group,
            ReviewText = model.ReviewText,
            Grade = model.Grade,
            CreationTime = DateTime.Now,
            Piece = piece,
            User = user,
            ImageUrl = "image"
        };

        foreach (var tagName in model.TagNames)
        {
            var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());

            if (existingTag == null)
            {
                // If tag doesn't exist, create it
                existingTag = new Tag { Name = tagName, Amount = 1 }; // You might need to set the amount appropriately
                _dbContext.Tags.Add(existingTag);
            }
            else
            {
                existingTag.Amount++;
            }

            // Associate the tag with the review
            var reviewTag = new ReviewTag { Review = review, Tag = existingTag };
            review.ReviewTags.Add(reviewTag);
        }
        
        user.Reviews.Add(review);
        piece.Reviews.Add((review));
        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();

        return Ok("Review created successfully");
    }
    
    [HttpGet("review-tags/{reviewId}")]
    public async Task<IActionResult> GetTagsForReview(int reviewId)
    {
        var review = await _dbContext.Reviews
            .Include(r => r.ReviewTags)
            .ThenInclude(rt => rt.Tag)
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

        if (review == null)
            return NotFound();

        var tags = review.ReviewTags.Select(rt => rt.Tag.Name);

        return Ok(tags);
    }
    
    [HttpPut("edit-review/{reviewId}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> EditReview(int reviewId, [FromBody] ReviewEditDto model)
    {
        var review = await _dbContext.Reviews.FindAsync(reviewId);

        if (review == null)
        {
            return NotFound("Review not found.");
        }

        if (User.IsInRole("Admin") || CurrentUserIsReviewAuthor(review))
        {
            review.ReviewName = model.ReviewName;
            review.Group = model.Group;
            review.ReviewText = model.ReviewText;
            review.Grade = model.Grade;

            _dbContext.Reviews.Update(review);
            await _dbContext.SaveChangesAsync();

            return Ok("Review updated successfully");
        }

        return Forbid("You are not authorized to edit this review.");
    }

    private bool CurrentUserIsReviewAuthor(Review review)
    {
        var currentUserId = GetCurrentUserId();
        return review.UserId == currentUserId;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }
        return -1; 
    }
    
    [Authorize(Roles = "Admin,User")]
    [HttpDelete("delete-review/{reviewId}")]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        var review = await _dbContext.Reviews.FindAsync(reviewId);

        if (review == null)
        {
            return NotFound("Review not found.");
        }

        if (User.IsInRole("Admin") || CurrentUserIsReviewAuthor(review))
        {
            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync();

            return Ok("Review deleted successfully");
        }

        return Forbid("You are not authorized to delete this review.");
    }
    
    // [Authorize(Roles = "Admin,User")]
    [HttpGet("get-review/{reviewId}")]
    public async Task<IActionResult> GetReview(int reviewId)
    {
        var review = await _dbContext.Reviews.FindAsync(reviewId);

        if (review == null)
        {
            return NotFound("Review not found.");
        }

        return Ok(review);
    }
    
    [HttpGet("get-all-reviews")]
    // [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetAllReviews()
    {
        var reviews = await _dbContext.Reviews.ToListAsync();
        return Ok(reviews);
    }

    [HttpGet("get-review-by-tag/{tagName}")]
    public async Task<IActionResult> GetReviewsByTag(string tagName)
    {
        var reviews = await _dbContext.Reviews
            .Where(r => r.ReviewTags.Any(rt => rt.Tag.Name == tagName))
            .ToListAsync();

        return Ok(reviews);
    }

    
    [HttpGet("get-review-by-user/{userId}")]
    public async Task<IActionResult> GetReviewsByUser(int userId)
    {
        var reviews = await _dbContext.Reviews
            .Where(r => r.User.UserId == userId)
            .ToListAsync();

        return Ok(reviews);
    }

    
    [HttpGet("get-review-by-piece/{pieceId}")]
    public async Task<IActionResult> GetReviewsByPiece(int pieceId)
    {
        var reviews = await _dbContext.Reviews
            .Where(r => r.Piece.PieceId == pieceId)
            .ToListAsync();

        return Ok(reviews);
    }
    
    [HttpGet("get-review-by-id/{reviewId}")]
    public async Task<IActionResult> GetReviewById(int reviewId)
    {
        var reviews = _dbContext.Reviews.SingleOrDefault(r => r.ReviewId == reviewId)!;
        return Ok(reviews);
    }

    
    [HttpGet("get-review-by-group/{group}")]
    public async Task<IActionResult> GetReviewsByGroup(Group group)
    {
        var reviews = await _dbContext.Reviews
            .Where(r => r.Group == group)
            .ToListAsync();

        return Ok(reviews);
    }
    
    [HttpPost("{reviewId}/like")]
    [Authorize]
    public async Task<IActionResult> LikeReview(int reviewId)
    {
        var review = await _dbContext.Reviews.FindAsync(reviewId);

        if (review == null)
        {
            return NotFound("Review not found.");
        }

        var currentUserId = GetCurrentUserId();

        if (review.Likes.Any(like => like.UserId == currentUserId))
        {
            return BadRequest("You have already liked this review.");
        }

        var like = new Like
        {
            ReviewId = reviewId,
            UserId = currentUserId
        };

        _dbContext.Likes.Add(like);
        await _dbContext.SaveChangesAsync();

        return Ok("Review liked successfully");
    }

    [HttpPost("{reviewId}/unlike")]
    [Authorize]
    public async Task<IActionResult> UnlikeReview(int reviewId)
    {
        var review = await _dbContext.Reviews.FindAsync(reviewId);

        if (review == null)
        {
            return NotFound("Review not found.");
        }

        var currentUserId = GetCurrentUserId();
        
        var like = review.Likes.FirstOrDefault(l => l.UserId == currentUserId);
        if (like == null)
        {
            return BadRequest("You have not liked this review.");
        }

        _dbContext.Likes.Remove(like);
        await _dbContext.SaveChangesAsync();

        return Ok("Review unliked successfully");
    }
    
}