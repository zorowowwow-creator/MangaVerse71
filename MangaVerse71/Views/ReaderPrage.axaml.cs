using Avalonia.Controls;
using MangaVerse71.Models;

namespace MangaVerse71.Views;

public partial class ReaderPage : UserControl
{
    public ReaderPage()
    {
        InitializeComponent();
    }

    public void LoadChapter(Chapter chapter)
    {
        DataContext = chapter;
    }
}





