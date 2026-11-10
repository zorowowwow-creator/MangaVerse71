using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MangaVerse71.Data;

public class MangaDbContextFactory
    : IDesignTimeDbContextFactory<MangaDbContext>
{
    public MangaDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MangaDbContext>();

        options.UseSqlite("Data Source=mangaverse.db");

        return new MangaDbContext(options.Options);
    }
}