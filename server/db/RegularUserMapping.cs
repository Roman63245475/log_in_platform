using LinqToDB.Mapping;
using be;

namespace db;

public class RegularUserMapping
{
    public static void Configure(FluentMappingBuilder builder)
    {
        builder.Entity<RegularUser>().HasTableName("users").HasPrimaryKey(x => x.id);
        builder.Entity<RegularUser>().Property(x => x.id).IsIdentity().HasColumnName("id");
        builder.Entity<RegularUser>().Property(x => x.username).HasColumnName("username");
        builder.Entity<RegularUser>().Property(x => x.password).HasColumnName("password");
    }
}
