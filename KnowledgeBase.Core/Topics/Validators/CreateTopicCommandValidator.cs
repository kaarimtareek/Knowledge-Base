using FluentValidation;
using KnowledgeBase.Core.ErrorMessages;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Interfaces.Validations;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.Topics.Validators;

public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    
    public CreateTopicCommandValidator(ITopicValidatorChecker topicValidator)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(TopicErrorMessages.NameRequired)
            .MaximumLength(100)
            .WithMessage(TopicErrorMessages.NameTooLong);
        
        RuleFor(x=> x)
            .MustAsync(
                async (command, cancellationToken)=> 
                    await topicValidator.IsTopicNameUniqueAsync(command.Name, null, cancellationToken))
            .WithMessage(TopicErrorMessages.AlreadyExists);

    }

}