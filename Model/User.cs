using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    [Required] [MaxLength(256)] public string Name { get; set; } = string.Empty;
    [Required] [MaxLength(256)] public string UserName { get; set; } = string.Empty;
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public Role Role { get; set; }
    public List<Like> Likes { get; set; } = new List<Like>();
    public List<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Comment> Comments { get; set; }

}