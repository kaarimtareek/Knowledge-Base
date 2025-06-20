using KnowledgeBase.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBase.API.Controllers.Base;

[ApiController]
public abstract class AppControllerBase : ControllerBase
{
    protected IApplicationDbContext DbContext;
    protected readonly IMediator Mediator;
    protected AppControllerBase(IApplicationDbContext dbContext, IMediator mediator)
    {
        DbContext = dbContext;
        Mediator = mediator;
        // This constructor can be used for any common setup needed for all controllers
    }
}