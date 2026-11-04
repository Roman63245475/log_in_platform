using be;
using LinqToDB;

namespace db;

public class DatabaseSeeder
{
    public static void SeedData(Database db)
    {
        SeedRoles(db);
        // Users will be seeded from Program.cs with proper PasswordHasher
    }

    private static void SeedRoles(Database db)
    {
        try
        {
            // Check if roles already exist
            if (db.GetTable<Role>().Any())
            {
                System.Console.WriteLine("Roles already exist, skipping seeding");
                return;
            }

            // Insert roles
            db.Insert(new Role { id = 1, role_name = "User" });
            db.Insert(new Role { id = 2, role_name = "SuperUser" });
            db.Insert(new Role { id = 3, role_name = "Admin" });
            
            System.Console.WriteLine("✓ Roles seeded successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error seeding roles: {ex.Message}");
        }
    }
}
