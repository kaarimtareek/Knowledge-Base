using Common;
using KnowledgeBase.API.Controllers.Base;
using KnowledgeBase.API.DTOs;
using KnowledgeBase.Core.CommandHandlers.Topics;
using KnowledgeBase.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TopicsController : AppControllerBase
{
    public TopicsController(IApplicationDbContext dbContext, IMediator mediator) : base(dbContext, mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        //this inline queries will be replaced with a query handler in the future
        var topics = await DbContext.Topics.AsNoTracking()
            .Select(x => new TopicDto(x.Id, x.Name, x.CreatedAt, x.UpdatedAt)).ToListAsync(cancellationToken);
        return Ok(topics);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        //this inline queries will be replaced with a query handler in the future
        var topic = await DbContext.Topics.AsNoTracking()
            .Select(x => new TopicDto(x.Id, x.Name, x.CreatedAt, x.UpdatedAt))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (topic == null)
            return NotFound(ApiResponse<TopicDto>.Failure(ErrorMessages.NotFound(nameof(TopicDto))));

        return Ok(ApiResponse<TopicDto>.Success(topic));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTopicCommand command, CancellationToken cancellationToken)
    {
        var topicId = await Mediator.Send(command, cancellationToken);

        var topicDto = await DbContext.Topics.AsNoTracking()
            .Where(x => x.Id == topicId)
            .Select(x => new TopicDto(x.Id, x.Name, x.CreatedAt, x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = topicId }, topicDto);
    }
}