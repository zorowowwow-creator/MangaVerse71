using System;
using System.Collections.Generic;
using System.Text;

namespace MangaVerse71.Models
{
    public class ReadingProgress
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MangaId { get; set; }

        public int LastChapterRead { get; set; }

        public DateTime LastReadAt { get; set; } = DateTime.Now;

        public double ProgressPercentage { get; set; }
    }
}