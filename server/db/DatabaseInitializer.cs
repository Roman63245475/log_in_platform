using be;
using LinqToDB;
using LinqToDB.Data;

namespace db;

public class DatabaseInitializer
{
    public static void Initialize(Database db)
    {
        try
        {
            db.CreateTable<User>(
                tableOptions: TableOptions.CreateIfNotExists
            );
            db.CreateTable<Role>(
                tableOptions: TableOptions.CreateIfNotExists
            );
            db.Execute(
                "CREATE UNIQUE INDEX IF NOT EXISTS users_username_unique ON users (username)"
            );
            
            System.Console.WriteLine("Database tables created successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error creating tables: {ex.Message}");
            throw;
        }
        
        // Seed initial data
        try
        {
            System.Console.WriteLine("Starting database seeding...");
            DatabaseSeeder.SeedData(db);
            System.Console.WriteLine("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error during seeding: {ex.Message}");
            throw;
        }
    }
}
