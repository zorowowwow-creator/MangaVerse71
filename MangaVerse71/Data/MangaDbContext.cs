using Microsoft.EntityFrameworkCore;
using MangaVerse71.Models;

namespace MangaVerse71.Data;

public class MangaDbContext : DbContext
{
    public DbSet<Manga> MangaList { get; set; }

    public DbSet<Comics> ComicList { get; set; }

    public DbSet<Favorite> Favorites { get; set; }

    public MangaDbContext(DbContextOptions<MangaDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=mangaverse.db");
        }
    }
}


