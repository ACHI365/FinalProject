using FinalProject.Data;
using FinalProject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controller;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly DataContext _dbContext;

    public TagController(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("get-all")]
    public IActionResult GetAllTags()
    {
        var tags = _dbContext.Tags.ToList();
        return Ok(tags);
    }
    
    [HttpGet("suggest/{partialTagName}")]
    public IActionResult SuggestTags(string partialTagName)
    {
        // Fetch tag suggestions based on the partial tag name
        var suggestions = _dbContext.Tags
            .Where(tag => tag.Name.Contains(partialTagName))
            .Select(tag => tag.Name)
            .ToList();

        return Ok(suggestions);
    }

    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public IActionResult CreateTag([FromBody] Tag tag)
    {
        // Check if the tag already exists
        var existingTag = _dbContext.Tags.FirstOrDefault(t => t.Name.Equals(tag.Name, StringComparison.OrdinalIgnoreCase));

        if (existingTag != null)
        {
            existingTag.Amount++;
            _dbContext.Tags.Update(existingTag);
            _dbContext.SaveChanges();
            return Ok("Tag already exists.");
        }

        _dbContext.Tags.Add(tag);
        _dbContext.SaveChanges();
        return Ok("Tag created successfully");
    }

    [HttpGet("{tagName}")]
    public IActionResult GetTagByName(string tagName)
    {
        var tag = _dbContext.Tags.FirstOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        if (tag == null)
        {
            return NotFound("Tag not found.");
        }

        return Ok(tag);
    }
}
