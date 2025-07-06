namespace KnowledgeBase.Core.Interfaces.Validations;

public interface ITopicValidatorChecker
{
    
    Task<bool> IsTopicNameUniqueAsync(string name, Guid? id = null, CancellationToken cancellationToken = default);
}