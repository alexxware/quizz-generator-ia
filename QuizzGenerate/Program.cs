using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizzGenerate.Configuration;
using QuizzGenerate.Dto.login;
using QuizzGenerate.Dto.register;
using QuizzGenerate.Mappers;
using QuizzGenerate.Models;
using QuizzGenerate.Repository;
using QuizzGenerate.Repository.supabase;
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

// jwt
/*
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var baseUrl = builder.Configuration["Supabase:Url"];
    options.Authority = $"{baseUrl}/auth/v1";
    options.RequireHttpsMetadata = true;

    var supabaseJwtSecret = builder.Configuration["Supabase:JwtSecret"];
    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(supabaseJwtSecret!));
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = "authenticated",
        ValidateIssuer = true,
        ValidIssuer = options.Authority,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signInKey
    };
});*/

var supabaseUrl = builder.Configuration["Supabase:Url"];
var jwtSecret = builder.Configuration["Supabase:JwtSecret"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //var baseUrl = builder.Configuration["Supabase:Url"];
        //options.Authority = $"{baseUrl}/auth/v1";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = $"{supabaseUrl}/auth/v1",
            ValidAudience = "authenticated",
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

// CORS
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
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<ISupabaseService, SupabaseService>();

// REPOSITORY
builder.Services.AddScoped<IGeminiRepository, GeminiRepository>();
builder.Services.AddScoped<ISupabaseRepository, SupabaseRepository>();

// VALIDATORS
builder.Services.AddScoped<IValidator<RegisterRequestDto>, RegisterUserValidator>();
builder.Services.AddScoped<IValidator<LoginRequestDto>, LoginUserValidator>();

// SUPABASE
/*builder.Services.Configure<SupabaseSettings>(
    builder.Configuration.GetSection("Supabase"));

builder.Services.AddSingleton<Client>(sp =>
{
    var config = sp.GetRequiredService<IOptions<SupabaseSettings>>().Value;
    var options = new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    };
    var client = new Client(config.Url, config.AnonKey, options);
    client.InitializeAsync();
    return client;
});
*/
builder.Services.Configure<SupabaseSettings>(
    builder.Configuration.GetSection("Supabase"));

builder.Services.AddSingleton<Client>(sp =>
{
    var cfg = sp.GetRequiredService<IOptions<SupabaseSettings>>().Value;
    var opts = new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = false
    };

    return new Client(cfg.Url, cfg.AnonKey, opts);
});
// AUTO MAPPER
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var supabase = app.Services.GetRequiredService<Client>();
await supabase.InitializeAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAngularDev");

app.MapControllers();

app.Run();