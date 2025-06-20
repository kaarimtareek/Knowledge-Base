namespace KnowledgeBase.Core.Models.Base;

public abstract class BaseEntity : ISoftDeletable
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
    }
}