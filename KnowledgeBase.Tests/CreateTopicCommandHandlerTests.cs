using FluentAssertions;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Topics;
using KnowledgeBase.Core.Topics.Validators;
using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Tests;

public class CreateTopicCommandHandlerTests
{
    private readonly IApplicationDbContext _context;

    private CreateTopicCommandValidator _validator;

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
        var result = await handler.Handle(command, CancellationToken.None);

        var topicId = result.Data;
        // Assert
        result.IsSuccess.Should().BeTrue();
        // We can now query the in-memory database to verify the result
        var createdTopic = await _context.Topics.FindAsync(topicId);
        createdTopic.Should().NotBeNull();
        createdTopic.Name.Should().Be("Test Topic");
    }

    [Fact]
    public async Task Handle_Should_HaveError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateTopicCommand(""); // Invalid command with empty name
        _validator = new CreateTopicCommandValidator(_context);
        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateTopicCommand.Name));
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("name is required.");
    }

    [Fact]
    public async Task Handle_Should_HaveError_WhenNameIsTooLong()
    {
        // Arrange
        var command =
            new CreateTopicCommand(new string('a', 101)); // Invalid command with name longer than 100 characters
        _validator = new CreateTopicCommandValidator(_context);
        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateTopicCommand.Name));
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be("Name must not exceed 100 characters.");
    }
}