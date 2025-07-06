using FluentValidation;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Interfaces.Validations;

namespace KnowledgeBase.Core.Topics.Validators;

public class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
{
    public UpdateTopicCommandValidator(ITopicValidatorChecker topicChecker)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(TopicErrorMessages.NameRequired)
            .MaximumLength(100)
            .WithMessage(TopicErrorMessages.NameTooLong);
        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
                await topicChecker.IsTopicNameUniqueAsync(command.Name, command.Id, cancellationToken))
            .WithMessage(TopicErrorMessages.AlreadyExists)
            .WithName("Name");
    }
}