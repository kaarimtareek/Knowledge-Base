using KnowledgeBase.API.Extensions;
using KnowledgeBase.Core.CommandHandlers.Topics;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTopicCommand).Assembly));
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