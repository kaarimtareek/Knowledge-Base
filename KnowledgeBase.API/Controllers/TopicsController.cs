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
        var topics = await DbContext.Topics.AsNoTracking()
            .Select(x => new TopicDto(x.Id, x.Name, x.CreatedAt, x.UpdatedAt)).ToListAsync(cancellationToken);
        return Ok(topics);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var topic = await DbContext.Topics.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (topic == null)
        {
            return NotFound();
        }

        return Ok(TopicDto.FromEntity(topic));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTopicCommand command, CancellationToken cancellationToken)
    {
        var topicId = await Mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = topicId });
    }
}