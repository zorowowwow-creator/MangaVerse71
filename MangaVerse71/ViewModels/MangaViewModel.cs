using System.Collections.ObjectModel;
using MangaVerse71.Models;

namespace MangaVerse71.ViewModels
{
    public class MangaViewModel
    {
        public ObservableCollection<Manga> MangaList { get; set; }

        public MangaViewModel()
        {
            MangaList = new ObservableCollection<Manga>();

            MangaList.Add(new Manga
            {
                Title = "One Piece",
                Author = "Eiichiro Oda",
                Description = "История о поиске сокровища One Piece."
            });

            MangaList.Add(new Manga
            {
                Title = "Tokyo Ghoul",
                Author = "Sui Ishida",
                Description = "История Кена Канеки."
            });

            MangaList.Add(new Manga
            {
                Title = "Naruto",
                Author = "Masashi Kishimoto",
                Description = "История юного ниндзя."
            });
        }
    }
}