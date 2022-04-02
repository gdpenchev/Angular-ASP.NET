namespace CuriousReadersAPI.Infrastructure;

using CuriousReadersData;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class SeedDatabase
{
    public static void MigrateDatabase(string connectionString)
    {
        DbContextOptionsBuilder<LibraryDbContext> dbBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
        dbBuilder.UseSqlServer(connectionString);
        LibraryDbContext context = new LibraryDbContext(dbBuilder.Options);
        context.Database.Migrate();
    }
}
