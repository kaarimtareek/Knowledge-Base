using FluentAssertions;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Models;
using KnowledgeBase.Core.Topics;
using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Tests.Commands;

public class DeleteTopicCommandHandlerTests
{
    private readonly IApplicationDbContext _context;


    public DeleteTopicCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<KnowledgeBaseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
            .Options;
        _context = new KnowledgeBaseDbContext(options);

        // Mock the ITopicValidatorChecker
    }
    [Fact]
    public async Task  Handle_Should_CallDelete_WhenTopicExists()
    {
        // Arrange
        var topic = new Topic()
        {
            Name = "Test Topic",
        };
        await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();

        var command = new DeleteTopicCommand(topic.Id);
        var handler = new DeleteTopicCommandHandler(_context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(topic.Id);

        var deletedTopic = await _context.Topics.FindAsync(topic.Id);
        deletedTopic?.IsDeleted.Should().BeTrue(); // Topic should be deleted
    }
    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTopicDoesNotExist()
    {
        // Arrange
        var nonExistentTopicId = Guid.NewGuid();
        var command = new DeleteTopicCommand(nonExistentTopicId);
        var handler = new DeleteTopicCommandHandler(_context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(TopicErrorMessages.NotFound);
    }


}