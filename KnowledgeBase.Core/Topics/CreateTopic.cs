using System.ComponentModel.DataAnnotations;
using Common;
using Common.Constants;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Models;
using MediatR;

namespace KnowledgeBase.Core.Topics;

public record CreateTopicCommand(
    string Name) : IRequest<OperationResult<Guid>>;

public class CreateTopicCommandHandler : BaseHandler, IRequestHandler<CreateTopicCommand, OperationResult<Guid>>
{

    public CreateTopicCommandHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<OperationResult<Guid>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = new Topic
        {
            Name = request.Name.Trim(),
        };
        await _context.Topics.AddAsync(topic, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        if (topic.Id == Guid.Empty)
        {
            return OperationResult<Guid>.Failure(ErrorMessages.BadRequest("Topic"));
        }

        return OperationResult<Guid>.Success(topic.Id);
    }
}