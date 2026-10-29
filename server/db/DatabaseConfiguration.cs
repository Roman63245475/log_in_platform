using LinqToDB;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.Mapping;

namespace db;

public static class DatabaseConfiguration
{
    public static Database Create(string connectionString)
    {
        var mappingSchema = new MappingSchema();

        var builder = new FluentMappingBuilder(mappingSchema);
        
        RegularUserMapping.Configure(builder);
        RoleMapping.Configure(builder);

        builder.Build();

        var options = new DataOptions()
            .UsePostgreSQL(
                connectionString,
                PostgreSQLVersion.AutoDetect)
            .UseMappingSchema(mappingSchema);

        var typedOptions = new DataOptions<Database>(options);

        return new Database(typedOptions);
    }
}
