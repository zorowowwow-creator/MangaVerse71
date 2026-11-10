
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia.Media.Imaging;

namespace MangaVerse71.Models
{
    public class Manga
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CoverImage { get; set; } = string.Empty;

        public string Status { get; set; } = "Unknown";

        public int TotalChapters { get; set; }

        [NotMapped]
        public Bitmap? CoverBitmap { get; set; }
    }
}