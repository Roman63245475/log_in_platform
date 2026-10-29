using LinqToDB.Mapping;
using be;

namespace db;

public class RoleMapping
{
    public static void Configure(FluentMappingBuilder builder)
    {
        builder.Entity<Role>().HasTableName("roles").HasPrimaryKey(x => x.id);
        builder.Entity<Role>().Property(x => x.id).IsIdentity().HasColumnName("id");
        builder.Entity<Role>().Property(x => x.role_name).HasColumnName("role_name");
    }
}