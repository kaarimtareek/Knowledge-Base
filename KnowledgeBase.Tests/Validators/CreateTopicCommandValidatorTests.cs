using FluentAssertions;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Interfaces.Validations;
using KnowledgeBase.Core.Topics;
using KnowledgeBase.Core.Topics.Validators;
using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnowledgeBase.Tests.Validators;

public class CreateTopicCommandValidatorTests
{
    private readonly IMock<ITopicValidatorChecker> _topicValidatorCheckerMock;

    private CreateTopicCommandValidator _validator;

    public CreateTopicCommandValidatorTests()
    {
        // Mock the ITopicValidatorChecker
        _topicValidatorCheckerMock = new Mock<ITopicValidatorChecker>();
    }

    [Fact]
    public async Task Handle_Should_HaveError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateTopicCommand(""); // Invalid command with empty name
        _validator = new CreateTopicCommandValidator(_topicValidatorCheckerMock.Object);
        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateTopicCommand.Name));
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be(TopicErrorMessages.NameRequired);
    }

    [Fact]
    public async Task Handle_Should_HaveError_WhenNameIsTooLong()
    {
        // Arrange
        var command =
            new CreateTopicCommand(new string('a', 101)); // Invalid command with name longer than 100 characters
        _validator = new CreateTopicCommandValidator(_topicValidatorCheckerMock.Object);
        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateTopicCommand.Name));
        result.Errors?.FirstOrDefault()?.ErrorMessage.Should().Be(TopicErrorMessages.NameTooLong);
    }

    [Fact]
    public async Task Handle_Should_HaveError_WhenNameIsNull()
    {
        //Arrange
        var commnad = new CreateTopicCommand(null);
        _validator = new CreateTopicCommandValidator(_topicValidatorCheckerMock.Object);
        //Act
        var result = await _validator.ValidateAsync(commnad);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.FirstOrDefault()?.ErrorMessage.Should().Be(TopicErrorMessages.NameRequired);



    }
}