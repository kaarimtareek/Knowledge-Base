using System.ComponentModel.DataAnnotations;
using Common.Constants;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Models;
using MediatR;

namespace KnowledgeBase.Core.CommandHandlers.Topics;

public record CreateTopicCommand(
    [Required]
    [MaxLength(ModelConstants.MaxLength.Name)]
    string Name) : IRequest<Guid>;

public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTopicCommandHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = new Topic
        {
            Name = request.Name.Trim(),
        };
        await _context.Topics.AddAsync(topic, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return topic.Id;
    }
}