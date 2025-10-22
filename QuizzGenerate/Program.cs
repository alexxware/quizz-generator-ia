using QuizzGenerate.Models;
using QuizzGenerate.Repository;
using QuizzGenerate.Service;

var builder = WebApplication.CreateBuilder(args);

//user secrets
builder.Configuration
    .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();

// Add services to the container.

// SERVICES
builder.Services.Configure<ApiKeysOptions>(builder.Configuration.GetSection("ApiKeys"));
builder.Services.AddScoped<IGeminiRepository, GeminiRepository>();
builder.Services.AddScoped<IGeminiService, GeminiService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();