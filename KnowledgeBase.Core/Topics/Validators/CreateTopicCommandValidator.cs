using FluentValidation;
using KnowledgeBase.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Core.Topics.Validators;

public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreateTopicCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");
        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .WithMessage("A topic with this name already exists.");
    }
    private async Task<bool> BeUniqueNameAsync(string name, CancellationToken cancellationToken)
    {
        // This method should check the database to ensure the name is unique.
        // For example:
        return !await _context.Topics.AnyAsync(x => x.Name == name, cancellationToken);
    }
}