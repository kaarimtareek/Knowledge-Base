using FluentValidation;
using KnowledgeBase.API.Extensions;
using KnowledgeBase.Core;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Topics;
using KnowledgeBase.Core.Topics.Validators;
using KnowledgeBase.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTopicCommand).Assembly));

// 2. Register all validators from the Application assembly
builder.Services.AddValidatorsFromAssembly(typeof(CreateTopicCommandValidator).Assembly);

// 3. Register the ValidationBehavior as a pipeline behavior for MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<KnowledgeBaseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("KnowledgeBaseConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<KnowledgeBaseDbContext>());

builder.Services.AddControllers();
// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KnowledgeBase API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KnowledgeBase API v1");
        c.RoutePrefix = string.Empty;
    });
    await app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();