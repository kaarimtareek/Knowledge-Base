using Common;
using KnowledgeBase.API.Controllers.Base;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Topics;
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
    public async Task<IActionResult> GetAll([FromQuery] GetAllTopicsQuery query, CancellationToken cancellationToken)
    {
        var topics = await Mediator.Send(query, cancellationToken);
        return Ok(topics.ToSuccessApiResponse());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetTopicQuery(id), cancellationToken);
        if (!result.IsSuccess)
        {
            return NotFound(result.ToFailureApiResponse());
        }

        return Ok(result.ToSuccessApiResponse());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTopicCommand command, CancellationToken cancellationToken)
    {
        var topicId = await Mediator.Send(command, cancellationToken);
        if (!topicId.IsSuccess)
        {
            return BadRequest(topicId.ToFailureApiResponse());
        }

        var result = await Mediator.Send(new GetTopicQuery(topicId.Data), cancellationToken);
        if (!result.IsSuccess)
        {
            return NotFound(result.ToFailureApiResponse());
        }

        var apiResponse = result.ToSuccessApiResponse();
        return CreatedAtAction(nameof(GetById), new { id = topicId.Data }, apiResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTopicCommand command,
        CancellationToken cancellationToken)
    {
        //validate the command, should be done by FluentValidation
        if (string.IsNullOrEmpty(command.Name))
        {
            return BadRequest(ApiResponse<TopicDto>.Failure("Name cannot be empty."));
        }

        if (command.Name.Length > 100)
        {
            return BadRequest(ApiResponse<TopicDto>.Failure("Name cannot be longer than 100 characters."));
        }

        var existingTopic = await DbContext.Topics
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == command.Name.Trim() && x.Id != id, cancellationToken);
        if (existingTopic != null)
        {
            return BadRequest(ApiResponse<TopicDto>.Failure(ErrorMessages.AlreadyExists("Topic", command.Name.Trim())));
        }

        command.Id = id;
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return NotFound(result.ToFailureApiResponse());
        }

        var updatedTopic = await Mediator.Send(new GetTopicQuery(result.Data), cancellationToken);
        if (!updatedTopic.IsSuccess)
        {
            return NotFound(updatedTopic.ToFailureApiResponse());
        }

        return Ok(updatedTopic.ToSuccessApiResponse());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteTopicCommand(id), cancellationToken);
        if (!result.IsSuccess)
        {
            return NotFound(result.ToFailureApiResponse());
        }

        return Ok(result.ToSuccessApiResponse());
    }
}