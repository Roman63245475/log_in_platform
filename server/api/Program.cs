using be;
using be.Authorization;
using db;
using service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;
using LinqToDB;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?.Where(origin => !string.IsNullOrWhiteSpace(origin))
    .ToArray() ?? ["http://localhost:8088"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
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

// Seed users after building the app
SeedUsersWithRealHashes(database);

app.UseCors("React");
app.UseOpenApi();
app.UseSwaggerUi();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();

void SeedUsersWithRealHashes(Database db)
{
    try
    {
        // Check if users already exist
        var userCount = db.GetTable<User>().Count();
        if (userCount > 0)
        {
            Console.Error.WriteLine("✓ Users already exist, skipping seeding");
            return;
        }

        var passwordHasher = new PasswordHasher<User>();

        var usersToSeed = new[]
        {
            new { username = "admin", password = "admin123", roleId = UserRole.Admin },
            new { username = "superuser", password = "superuser123", roleId = UserRole.SuperUser },
            new { username = "john_doe", password = "password123", roleId = UserRole.User },
            new { username = "jane_smith", password = "password456", roleId = UserRole.User },
            new { username = "bob_wilson", password = "password789", roleId = UserRole.SuperUser },
            new { username = "alice_johnson", password = "password101", roleId = UserRole.User }
        };

        foreach (var userData in usersToSeed)
        {
            var user = new User
            {
                username = userData.username,
                role = userData.roleId
            };

            // Hash password using ASP.NET Identity PasswordHasher
            user.password_hash = passwordHasher.HashPassword(user, userData.password);

            // Insert into database
            db.Insert(user);
            Console.Error.WriteLine($"✓ Seeded user: {userData.username} with role {userData.roleId}");
        }

        Console.Error.WriteLine("\n✓ All users seeded successfully with proper hashes!");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error seeding users: {ex.Message}");
        Console.Error.WriteLine($"Stack trace: {ex.StackTrace}");
    }
}
