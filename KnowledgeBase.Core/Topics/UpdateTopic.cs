using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Common;
using Common.Constants;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.Topics;

public class UpdateTopicCommand(
    Guid Id,
    [Required] [MaxLength(ModelConstants.MaxLength.Name)]
    string Name) : IRequest<OperationResult<Guid>>
{
    [JsonIgnore]
    public Guid Id { get; set; } = Id;

    public string Name { get; } = Name;
}

public class UpdateTopicCommandHandler : BaseHandler, IRequestHandler<UpdateTopicCommand, OperationResult<Guid>>
{
    public UpdateTopicCommandHandler(IApplicationDbContext context) : base(context)
    {
    }


    public async Task<OperationResult<Guid>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await _context.Topics
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (topic == null)
        {
            return OperationResult<Guid>.Failure(TopicErrorMessages.NotFound);
        }

        topic.Name = request.Name.Trim();
        topic.Update();
        _context.Topics.Update(topic);
        await _context.SaveChangesAsync(cancellationToken);
        //Execute property is not suppoerted in testing database providers like SQLite or in-memory database.

        return OperationResult<Guid>.Success(request.Id);
    }
}