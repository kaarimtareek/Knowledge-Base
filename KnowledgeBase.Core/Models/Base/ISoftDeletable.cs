namespace KnowledgeBase.Core.Models.Base;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}