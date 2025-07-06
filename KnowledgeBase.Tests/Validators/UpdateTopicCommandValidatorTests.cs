using FluentAssertions;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Interfaces.Validations;
using KnowledgeBase.Core.Models;
using KnowledgeBase.Core.Topics;
using KnowledgeBase.Core.Topics.Validators;
using KnowledgeBase.Core.ValidationServices;
using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnowledgeBase.Tests.Validators;

public class UpdateTopicCommandValidatorTests
{
    private readonly IApplicationDbContext _context;

    private UpdateTopicCommandValidator _commandValidator;

    private readonly ITopicValidatorChecker _topicValidatorChecker;

    public UpdateTopicCommandValidatorTests()
    {
        var options = new DbContextOptionsBuilder<KnowledgeBaseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
            .Options;
        _context = new KnowledgeBaseDbContext(options);
        _topicValidatorChecker = new TopicValidatorChecker(_context);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenTopicNameAlreadyExists()
    {
        //Arrange
        await _context.Topics.AddAsync(new Topic
        {
            Name = "Test Topic"
        });
        var topic = new Topic
        {
            Name = "Another Topic"
        };
        await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();
        //update topic with the same name for another topic
        var command = new UpdateTopicCommand(topic.Id, "Test Topic");
        //Mock the behavior of validator checker
        _commandValidator = new UpdateTopicCommandValidator(_topicValidatorChecker);
        //Act
        var result = await _commandValidator.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(UpdateTopicCommand.Name));
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be(TopicErrorMessages.AlreadyExists);
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
        _commandValidator = new UpdateTopicCommandValidator(_topicValidatorChecker);
        //Act
        var result = await _commandValidator.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
    }
}