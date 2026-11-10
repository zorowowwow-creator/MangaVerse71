using System.Collections.Generic;

namespace MangaVerse71.Models;

public class Chapter
{
    public int Id { get; set; }

    public int MangaId { get; set; }

    public string Title { get; set; } = string.Empty;

    public int ChapterNumber { get; set; }

    public List<string> PageUrls { get; set; } = new();
}





