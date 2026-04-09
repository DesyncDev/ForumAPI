using Forum.Application;
using Forum.Infrastructure;
using ForumAPI.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Infra
builder.Services.AddInfraStructure(builder.Configuration);

// Application
builder.Services.AddApplication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// GlobalMiddleware
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();