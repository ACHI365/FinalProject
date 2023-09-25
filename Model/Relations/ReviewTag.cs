namespace FinalProject.Model.Relations;

public class ReviewTag
{
    public int ReviewId { get; set; }
    public Review Review { get; set; }

    public int TagId { get; set; }
    public Tag Tag { get; set; }
}
