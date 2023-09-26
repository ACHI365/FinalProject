using FinalProject.Config;
using FinalProject.Data;
using FinalProject.Model;
using FinalProject.Service.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FinalProject.Service;

public class TagService : ITagService
{
    private readonly DataContext _context;

    public TagService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tag?>> GetAllTagsAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task<bool> CreateTag(Tag? tag)
    {
        var existingTag =
            await _context.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tag.Name, StringComparison.OrdinalIgnoreCase));
        if (existingTag != null) UpdateTags(existingTag);
        else AddTag(tag);
        return existingTag == null;
    }

    public async Task<Tag?> GetTagByName(string tagName)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
    }

    private async void UpdateTags(Tag? existingTag)
    {
        existingTag.Amount++;
        _context.Tags.Update(existingTag);
        await _context.SaveChangesAsync();
    }

    private async void AddTag(Tag? tag)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }
}