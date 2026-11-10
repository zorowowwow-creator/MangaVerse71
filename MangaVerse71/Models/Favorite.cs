using System;

namespace MangaVerse71.Models;

public class Favorite
{
    public int Id { get; set; }

    public int MangaId { get; set; }

    public int ComicId { get; set; }

    public string ContentType { get; set; } = string.Empty;

    public DateTime AddedAt { get; set; } = DateTime.Now;
}