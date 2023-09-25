using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Model;

public class Piece
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PieceId { get; set; }
    public string Name { get; set; }
    public Group Group { get; set; }
    public double AverageRating { get; set; }

    public List<Review> Reviews { get; set; } = new List<Review>();
}
