using be;
using LinqToDB;
using LinqToDB.Data;

namespace db;

public class DatabaseInitializer
{
    public static void Initialize(Database db)
    {
        db.CreateTable<User>(
            tableOptions: TableOptions.CreateIfNotExists
        );
        db.Execute(
            "CREATE UNIQUE INDEX IF NOT EXISTS users_username_unique ON users (username)"
        );
    }
}
