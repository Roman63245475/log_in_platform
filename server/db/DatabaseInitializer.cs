using be;
using LinqToDB;

namespace db;

public class DatabaseInitializer
{
    public static void Initialize(Database db)
    {
        db.CreateTable<RegularUser>(
            tableOptions: TableOptions.CreateIfNotExists
        );
    }
}
