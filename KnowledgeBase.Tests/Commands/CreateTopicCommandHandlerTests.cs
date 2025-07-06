using FluentAssertions;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Interfaces.Validations;
using KnowledgeBase.Core.Topics;
using KnowledgeBase.Core.Topics.Validators;
using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnowledgeBase.Tests.Commands;

public class CreateTopicCommandHandlerTests
{
    private readonly IApplicationDbContext _context;

    public CreateTopicCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<KnowledgeBaseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
            .Options;
        _context = new KnowledgeBaseDbContext(options);

        // Mock the ITopicValidatorChecker
    }

    [Fact]
    public async Task Handle_Should_CallAddAsync_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateTopicCommand("Test Topic");
        var handler = new CreateTopicCommandHandler(_context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        var topicId = result.Data;
        // Assert
        result.IsSuccess.Should().BeTrue();
        // We can now query the in-memory database to verify the result
        var createdTopic = await _context.Topics.FindAsync(topicId);
        createdTopic.Should().NotBeNull();
        createdTopic.Name.Should().Be("Test Topic");
    }
}