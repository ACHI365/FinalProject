namespace FinalProject.Model.Dto;

public class ReviewEditDto
{
    public string ReviewName { get; set; }
    public Group Group { get; set; }
    public string ReviewText { get; set; }
    public double Grade { get; set; }
}