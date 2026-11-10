using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using MangaVerse71.Data;
using MangaVerse71.ViewModels;
using MangaVerse71.Views;

namespace MangaVerse71;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var options = new DbContextOptionsBuilder<MangaDbContext>()
                .UseSqlite("Data Source=mangaverse.db")
                .Options;

            var dbContext = new MangaDbContext(options);
            dbContext.Database.Migrate();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(dbContext),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}


