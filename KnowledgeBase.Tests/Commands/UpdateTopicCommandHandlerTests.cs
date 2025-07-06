using FluentAssertions;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Interfaces.Validations;
using KnowledgeBase.Core.Models;
using KnowledgeBase.Core.Topics;
using KnowledgeBase.Core.Topics.Validators;
using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnowledgeBase.Tests.Commands;

public class UpdateTopicCommandHandlerTest
{
    private readonly IApplicationDbContext _context;


    public UpdateTopicCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<KnowledgeBaseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
            .Options;
        _context = new KnowledgeBaseDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTopicNameAlreadyExists()
    {
        //Arrange
        var topic = new Topic();
        topic.Name = "Another Topic";
        await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();
        //update topic with the same name for another topic
        var command = new UpdateTopicCommand(topic.Id, "Test Topic");
        var handler = new UpdateTopicCommandHandler(_context);
        //Act

        var result = await handler.Handle(command, default);
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenTopicNameIsUnique()
    {
        //Arrange
        var topic = new Topic
        {
            Name = "Test Topic"
        };
        await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();
        //update topic with the same name for another topic
        var command = new UpdateTopicCommand(topic.Id, "Another Topic");
        var handler = new UpdateTopicCommandHandler(_context);
        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTopicNotExist()
    {
        //Arrange
        var topic = new Topic
        {
            Name = "Test Topic"
        };
        await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();
        var command = new UpdateTopicCommand(Guid.NewGuid(), "Another Topic");
        var handler = new UpdateTopicCommandHandler(_context);
        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(TopicErrorMessages.NotFound);
    }
}