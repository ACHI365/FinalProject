using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProject.Model.Relations;

namespace FinalProject.Model;

public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TagId { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    public List<ReviewTag> ReviewTags { get; set; } = new List<ReviewTag>();
}