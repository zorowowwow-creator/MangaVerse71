namespace MangaVerse71.Models;

public class Comics
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CoverImage { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}