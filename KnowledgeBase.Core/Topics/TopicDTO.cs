using KnowledgeBase.Core.Models;

namespace KnowledgeBase.Core.Topics;

public record TopicDto (Guid Id, string Name, DateTime CreatedAt = default, DateTime? UpdatedAt = null) 
{
    public static TopicDto FromEntity(Topic topic)
    {
        return new TopicDto(topic.Id, topic.Name,
            topic.CreatedAt, topic.UpdatedAt);
    }

    public Topic ToEntity()
    {
        return new Topic
        {
            Id = Id,
            Name = Name
        };
    }
};
