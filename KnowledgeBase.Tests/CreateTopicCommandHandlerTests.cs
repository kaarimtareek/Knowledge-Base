using FluentAssertions;
using KnowledgeBase.Core.CommandHandlers.Topics;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Models;
using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnowledgeBase.Tests;

public class CreateTopicCommandHandlerTests
{
    private readonly IApplicationDbContext _context;

    public CreateTopicCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<KnowledgeBaseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
            .Options;
        _context = new KnowledgeBaseDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_CallAddAsyncOnRepository_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateTopicCommand("Test Topic");
        var handler = new CreateTopicCommandHandler(_context);

        // Act
        var topicId = await handler.Handle(command, CancellationToken.None);

        // Assert
        // We can now query the in-memory database to verify the result
        var createdTopic = await _context.Topics.FindAsync(topicId);
        createdTopic.Should().NotBeNull();
        createdTopic.Name.Should().Be("Test Topic");
    }
}