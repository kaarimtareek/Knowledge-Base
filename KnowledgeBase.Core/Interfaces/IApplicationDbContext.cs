using KnowledgeBase.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Topic> Topics { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}