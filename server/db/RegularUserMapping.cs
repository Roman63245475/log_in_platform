using LinqToDB.Mapping;
using be;

namespace db;

public class RegularUserMapping
{
    public static void Configure(FluentMappingBuilder builder)
    {
        builder.Entity<User>().HasTableName("users").HasPrimaryKey(x => x.id);
        builder.Entity<User>().Property(x => x.id).IsIdentity().HasColumnName("id");
        builder.Entity<User>().Property(x => x.username).HasColumnName("username");
        builder.Entity<User>().Property(x => x.password).HasColumnName("password");
    }
}
