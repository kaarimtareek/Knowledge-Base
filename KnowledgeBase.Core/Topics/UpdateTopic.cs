using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Common;
using Common.Constants;
using KnowledgeBase.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.Topics;

public class UpdateTopicCommand(
    Guid Id,
    [Required]
    [MaxLength(ModelConstants.MaxLength.Name)]
    string Name) :  IRequest<OperationResult<Guid>>
{
    [JsonIgnore]
    public Guid Id { get; set; } = Id;
    public string Name { get; } = Name;
}

public class UpdateTopicHandler : BaseHandler, IRequestHandler<UpdateTopicCommand, OperationResult<Guid>>
{
    public UpdateTopicHandler(IApplicationDbContext context) : base(context)
    {
    }


    public async Task<OperationResult<Guid>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var updatedNumber = await _context.Topics
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Name, request.Name.Trim())
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
        if (updatedNumber == 0)
        {
            return OperationResult<Guid>.Failure(ErrorMessages.NotFound("Topic"));
        }

        return OperationResult<Guid>.Success(request.Id);
    }
}