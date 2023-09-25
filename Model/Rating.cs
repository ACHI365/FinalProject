namespace FinalProject.Model;

public class Rating
{
    public int RatingId { get; set; }
    public int PieceId { get; set; }
    public Piece Piece { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int Score { get; set; }
    public DateTime DateRated { get; set; }
}