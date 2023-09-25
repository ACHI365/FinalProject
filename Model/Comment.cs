using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Model;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CommentId { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public int ReviewId { get; set; }
    public Review Review { get; set; }
    [ForeignKey("UserId")] public int UserId { get; set; }
    public User User { get; set; }
}