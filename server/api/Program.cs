using db;

var builder = WebApplication.CreateBuilder(args);
var connection_string = builder.Configuration.GetConnectionString("connection_string")
    ?? throw new InvalidOperationException("Connection string 'connection_string' not found.");
var database = DatabaseConfiguration.Create(connection_string);
DatabaseInitializer.Initialize(database);
builder.Services.AddScoped<Database>(
    _ => DatabaseConfiguration.Create(connection_string)
);

var app = builder.Build();



app.Run();
