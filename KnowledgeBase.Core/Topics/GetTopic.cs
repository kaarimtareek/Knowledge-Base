using Common;
using KnowledgeBase.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.Topics;

public record GetTopicQuery(
    Guid Id) : IRequest<OperationResult<TopicDto>>;

public class GetTopicQueryHandler : BaseHandler, IRequestHandler<GetTopicQuery, OperationResult<TopicDto>>
{
    public GetTopicQueryHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<OperationResult<TopicDto>> Handle(GetTopicQuery request, CancellationToken cancellationToken)
    {
        var topic = await _context.Topics.AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => TopicDto.FromEntity(x))
            .FirstOrDefaultAsync(cancellationToken);
        if (topic == null)
        {
            return OperationResult<TopicDto>.Failure(ErrorMessages.NotFound("Topic"));
        }

        return OperationResult<TopicDto>.Success(topic);
    }
}