using FinalProject.Model;

namespace FinalProject.Service.ServiceInterface;

public interface ITagService
{
    Task<IEnumerable<Tag?>> GetAllTagsAsync();
    Task<bool> CreateTag(Tag? tag);
    Task<Tag?> GetTagByName(string tagName);
}