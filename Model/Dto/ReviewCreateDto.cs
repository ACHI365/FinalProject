namespace FinalProject.Model.Dto;

public class ReviewCreateDto
{
    public string ReviewName { get; set; }
    public Group Group { get; set; }
    public string ReviewText { get; set; }
    public double Grade { get; set; }
    // public string ImageUrl { get; set; }
    public List<string> TagNames { get; set; }  // New property for tag names
    public int PieceId { get; set; }
    public int UserId { get; set; }
}