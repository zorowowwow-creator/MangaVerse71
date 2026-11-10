using System;
using System.Collections.Generic;
using System.Text;

namespace MangaVerse71.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Biography { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
    }
}