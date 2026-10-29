using System.IdentityModel.Tokens.Jwt;
using api;
using be.Authorization;
using db;
using service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(document =>
{
    document.AddSecurity(
        "Bearer",
        Enumerable.Empty<string>(),
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter a JWT bearer token.",
        });
    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

var connection_string = builder.Configuration.GetConnectionString("connection_string")
    ?? throw new InvalidOperationException("Connection string 'connection_string' not found.");
var database = DatabaseConfiguration.Create(connection_string);
DatabaseInitializer.Initialize(database);
builder.Services.AddScoped<Database>(
    _ => DatabaseConfiguration.Create(connection_string)
);
builder.Services.AddScoped<LoginController>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IAuthorizationService, JWTAuthorizationSerivice>();
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]
                    ?? throw new InvalidOperationException("JWT:Secret is not configured.")))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("React");
app.UseOpenApi();
app.UseSwaggerUi();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
