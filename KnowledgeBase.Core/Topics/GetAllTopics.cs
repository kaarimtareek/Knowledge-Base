using Common;
using KnowledgeBase.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.Topics;

public record GetAllTopicsQuery : BaseQuery, IRequest<OperationResult<IList<TopicDto>>>;

public class GetAllTopicsQueryHandler : BaseHandler,
    IRequestHandler<GetAllTopicsQuery, OperationResult<IList<TopicDto>>>
{
    public GetAllTopicsQueryHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<OperationResult<IList<TopicDto>>> Handle(GetAllTopicsQuery request,
        CancellationToken cancellationToken)
    {
        var topics = _context.Topics.AsNoTracking();
        if (!string.IsNullOrEmpty(request.Search))
        {
            topics = topics.Where(x => x.Name.Contains(request.Search.Trim()));
        }

        topics = request.SortBy switch
        {
            "Name" => request.SortDescending ? topics.OrderByDescending(x => x.Name) : topics.OrderBy(x => x.Name),
            _ => topics
        };

        var result = await topics.Select(x => TopicDto.FromEntity(x)).ToListAsync(cancellationToken);
        return OperationResult<IList<TopicDto>>.Success(result);
    }
}