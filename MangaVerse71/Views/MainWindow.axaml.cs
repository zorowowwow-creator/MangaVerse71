using Avalonia.Controls;
using MangaVerse71.Data;
using MangaVerse71.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MangaVerse71.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var options = new DbContextOptionsBuilder<MangaDbContext>()
            .UseSqlite("Data Source=mangaverse.db")
            .Options;

        var dbContext = new MangaDbContext(options);

        DataContext = new MainViewModel(dbContext);
    }
}





