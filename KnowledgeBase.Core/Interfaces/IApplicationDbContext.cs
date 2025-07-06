using KnowledgeBase.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace KnowledgeBase.Core.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Topic> Topics { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
    int SaveChanges();
}