using MeuProjeto.Api.Extensions;
using MeuProjeto.Application.Extensions;
using MeuProjeto.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMyApplication();
builder.Services.AddMyInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddMyHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCustomExceptionHandler();

app.MapControllers();

await app.InitializeRabbitMqAsync();

app.Run();
