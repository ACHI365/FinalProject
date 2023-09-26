using System.Diagnostics.CodeAnalysis;
using FinalProject.Data;
using FinalProject.Model;
using FinalProject.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controller;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly DataContext _dbContext;
    private readonly ITagService _tagService;

    public TagController(DataContext dbContext, ITagService tagService)
    {
        _tagService = tagService;
        _dbContext = dbContext;
    }

    [HttpGet("get-all")]
    public IActionResult GetAllTags()
    {
        var tags = _tagService.GetAllTagsAsync();
        return Ok(tags);
    }
    
    [HttpPost]
    [Authorize]
    public IActionResult CreateTag([FromBody] Tag? tag)
    {
        var newTag = _tagService.CreateTag(tag);
        return newTag.Result ? Ok("Tag created successfully") : Ok("Tag already exists");
    }

    [HttpGet("{tagName}")]
    public IActionResult GetTagByName(string tagName)
    {
        var tag = _tagService.GetTagByName(tagName);
        return tag.Result == null ? NotFound("Tag does not exists") : Ok(tag);
    }
}
