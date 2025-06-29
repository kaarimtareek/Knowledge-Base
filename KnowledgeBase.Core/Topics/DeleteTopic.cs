using Common;
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
        var updatedNumber = await _context.Topics.Where(x => x.Id == request.Id).ExecuteUpdateAsync(s => s
            .SetProperty(x =>
                x.IsDeleted, true).SetProperty(x => x.UpdatedAt, DateTime.Now), cancellationToken);

        if(updatedNumber == 0)
        {
            return OperationResult<Guid>.Failure(ErrorMessages.NotFound("Topic"));
        }

        return OperationResult<Guid>.Success(request.Id);
    }
}