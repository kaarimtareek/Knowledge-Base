using KnowledgeBase.Core.Interfaces;

namespace KnowledgeBase.Core;

public abstract class BaseHandler
{
    
    protected readonly IApplicationDbContext _context;

    protected BaseHandler(IApplicationDbContext context)
    {
        _context = context;
    }
}