using FluentValidation;
using QuizzGenerate.Dto.register;
using QuizzGenerate.Models;
using QuizzGenerate.Repository;
using QuizzGenerate.Service;
using QuizzGenerate.Validators;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

//user secrets
builder.Configuration
    .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();

//config front
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
// Add services to the container.

// SERVICES
builder.Services.Configure<ApiKeysOptions>(builder.Configuration.GetSection("ApiKeys"));
builder.Services.AddScoped<IGeminiRepository, GeminiRepository>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<ISupabaseService, SupabaseService>();

// VALIDATORS
builder.Services.AddScoped<IValidator<RegisterRequestDto>, RegisterUserValidator>();

// SUPABASE
builder.Services.AddScoped<Supabase.Client>(_ => new Client(
    builder.Configuration["Supabase:Url"],
    builder.Configuration["Supabase:SupKey"],
    new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    }
    ));

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

app.UseCors("AllowAngularDev");

app.MapControllers();

app.Run();