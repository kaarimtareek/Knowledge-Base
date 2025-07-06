using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Interfaces.Validations;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.ValidationServices;

public class TopicValidatorChecker : ITopicValidatorChecker
{
    private readonly IApplicationDbContext _context;

    public TopicValidatorChecker(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsTopicNameUniqueAsync(string name, Guid? id = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await _context.Topics.AsNoTracking()
            .Where(x => x.Name == name && (!id.HasValue || x.Id != id))
            .AnyAsync(cancellationToken);
        return !exists;
    }
}