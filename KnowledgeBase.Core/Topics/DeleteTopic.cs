using Common;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.Topics;

public record DeleteTopicCommand(
    Guid Id) : IRequest<OperationResult<Guid>>;

public class DeleteTopicCommandHandler : BaseHandler, IRequestHandler<DeleteTopicCommand, OperationResult<Guid>>
{
    public DeleteTopicCommandHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<OperationResult<Guid>> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await _context.Topics
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (topic == null)
        {
            return OperationResult<Guid>.Failure(TopicErrorMessages.NotFound);
        }
        topic.Delete();
        _context.Topics.Update(topic);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<Guid>.Success(request.Id);
    }
}