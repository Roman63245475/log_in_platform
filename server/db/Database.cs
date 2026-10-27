using LinqToDB;
using LinqToDB.Data;

namespace db;

public class Database: DataConnection
{
    
    public Database(DataOptions<Database> options) : base(options.Options)
    {
        
    }
}
