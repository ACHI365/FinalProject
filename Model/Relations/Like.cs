using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Model;

public class Like
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LikeId { get; set; }

    public int ReviewId { get; set; }
    public Review Review { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}