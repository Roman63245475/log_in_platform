using api;
using db;
using service;

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
builder.Services.AddOpenApiDocument();

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
builder.Services.AddControllers();
var app = builder.Build();

app.UseCors("React");
app.UseOpenApi();
app.UseSwaggerUi();
app.MapControllers();
app.Run();
