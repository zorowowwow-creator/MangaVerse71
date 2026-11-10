using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MangaVerse71.Data;
using MangaVerse71.Models;
using MangaVerse71.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
namespace MangaVerse71.ViewModels;


public partial class MainViewModel : ViewModelBase
{
    private readonly MangaDbContext _dbContext;

    public MainViewModel(MangaDbContext dbContext)
    {
        _dbContext = dbContext;
        _ = LoadLibraryAsync();
    }

    private async Task LoadLibraryAsync()
    {
        var mangas = _dbContext.MangaList.ToList();

        foreach (var manga in mangas)
        {
            if (!string.IsNullOrWhiteSpace(manga.CoverImage))
            {
                manga.CoverBitmap = await LoadCoverAsync(manga.CoverImage);
            }

            MangaList.Add(manga);
        }
    }

    [RelayCommand]
    private void OpenManga()
    {
        CurrentPage = new MangaSearchPage { DataContext = this };
    }

    [RelayCommand]
    private void OpenComics()
    {
        CurrentPage = new ComicsSearchPage { DataContext = this };
    }

    [ObservableProperty]
    private Manga? selectedManga;  

    [RelayCommand]
    private void OpenMangaDetails(Manga manga)
    {
        SelectedManga = manga;
        CurrentPage = new MangaDetailsPage
        {
            DataContext = this
        };
    }
    private static  readonly HttpClient _HttpClient = new HttpClient();
    public ObservableCollection<Manga> ComicResults { get; } = new();

    [ObservableProperty]
    public partial string Greeting { get; set; } = " Добро пожаловать в MangaVerse!";

    [ObservableProperty]
    public partial string LibraryTitle { get; set; } = "My Library";

    [ObservableProperty]
    public partial string SearchText { get; set; } = "Search Manga...";

    [ObservableProperty]
    private UserControl? _currentPage;


    [ObservableProperty]
    public partial string NewMangaTitle { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewMangaAuthor { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewMangaDescription { get; set; } = string.Empty;


    public ObservableCollection<Manga> MangaList { get; } = new();

    private static string CleanDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return "Описание отсутствует.";

        description = Regex.Replace(
            description,
            @"<br\s*/?>|</p>|</li>",
            "\n",
            RegexOptions.IgnoreCase);

        description = Regex.Replace(description, @"<[^>]+>", "");

        description = WebUtility.HtmlDecode(description);

        return description.Trim();
    }

    [ObservableProperty]
    private string comicSearchText = string.Empty;

    [ObservableProperty]
    private string comicSearchError = string.Empty;

    [ObservableProperty]
    private bool isComicSearching;
    public ObservableCollection<Manga> SearchResults { get; } = new();

    [ObservableProperty]
    public partial string MangaSearchText { get; set; } = string.Empty;


    [ObservableProperty]
    public partial string SearchError { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsSearching { get; set; }

    private static readonly HttpClient Http = new HttpClient();

    
    [RelayCommand]
 
    private async Task SearchMangaAsync()
    {
        SearchResults.Clear();
        SearchError = string.Empty;

        if (string.IsNullOrWhiteSpace(MangaSearchText))
        {
            SearchError = "Enter a title.";
            return;
        }

        IsSearching = true;

        try
        {
            using var client = new HttpClient();

            var query = """
        query ($search: String) {
            Page(perPage: 20) {
                media(search: $search, type: MANGA) {
                    title {
                        romaji
                        english
                    }
                    description
                    coverImage {
                        large
                    }
                }
            }
        }
        """;

            var request = new
            {
                query,
                variables = new { search = MangaSearchText }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(request);

            var response = await client.PostAsync(
                "https://graphql.anilist.co",
                new StringContent(json,
                    System.Text.Encoding.UTF8,
                    "application/json"));

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            using var document =
                System.Text.Json.JsonDocument.Parse(result);

            var media = document.RootElement
                .GetProperty("data")
                .GetProperty("Page")
                .GetProperty("media");

            foreach (var item in media.EnumerateArray())
            {
                var title = item.GetProperty("title");

                string name =
                    title.GetProperty("english").GetString()
                    ?? title.GetProperty("romaji").GetString()
                    ?? "Unknown";

                string description =
                    item.GetProperty("description").GetString() ?? "";

                description = System.Text.RegularExpressions.Regex.Replace(
                    description, "<.*?>", "");

                SearchResults.Add(new Manga
                {
                    Title = name,
                    Description = description
                });
            }

            if (SearchResults.Count == 0)
                SearchError = "Nothing found.";
        }
        catch (Exception ex)
        {
            SearchError = ex.Message;
        }
        finally
        {
            IsSearching = false;
        }
    }



    private async Task<string> TranslateDescriptionAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var result = new List<string>();

        while (text.Length > 0)
        {
            int length = Math.Min(450, text.Length);

            if (length < text.Length)
            {
                int space = text.LastIndexOf(' ', length - 1, length);
                if (space > 0)
                    length = space;
            }

            string part = text[..length].Trim();
            text = text[length..].TrimStart();

            string url =
                "https://api.mymemory.translated.net/get?q="
                + Uri.EscapeDataString(part)
                + "&langpair=en%7Cru";

            string json = await Http.GetStringAsync(url);

            using var document = JsonDocument.Parse(json);

            string translated = document.RootElement
                .GetProperty("responseData")
                .GetProperty("translatedText")
                .GetString() ?? part;

            result.Add(translated);
        }

        return string.Join(" ", result);
    }








    [RelayCommand]
    private void GoHome()
    {
        CurrentPage = null;
    }

    [RelayCommand]
    private async Task AddManga()
    {
        if (string.IsNullOrWhiteSpace(NewMangaTitle))
            return;

        var manga = new Manga
        {
            Title = NewMangaTitle.Trim(),
            Author = NewMangaAuthor.Trim(),
            Description = NewMangaDescription.Trim()
        };

        try
        {
            await _dbContext.MangaList.AddAsync(manga);
            await _dbContext.SaveChangesAsync();

            MangaList.Add(manga);

            NewMangaTitle = string.Empty;
            NewMangaAuthor = string.Empty;
            NewMangaDescription = string.Empty;
        }
        catch (Exception ex)
        {
            SearchError = "Ошибка сохранения: " + ex.Message;
        }
    }


    [RelayCommand]
private async Task AddSearchManga(Manga? manga)
{
    if (manga == null)
        return;

    bool exists = await _dbContext.MangaList
        .AnyAsync(m => m.Title == manga.Title);

    if (exists)
    {
        SearchError = "Эта манга уже в библиотеке.";
        return;
    }

    await _dbContext.MangaList.AddAsync(manga);
    await _dbContext.SaveChangesAsync();

    MangaList.Add(manga);
}



    [RelayCommand]
    private async Task SearchComicsAsync()
    {
        ComicResults.Clear();
        ComicSearchError = string.Empty;

        if (string.IsNullOrWhiteSpace(ComicSearchText))
        {
            ComicSearchError = "Введите название комикса.";
            return;
        }

        IsComicSearching = true;

        try
        {
            string url = "https://openlibrary.org/search.json?q="
                + Uri.EscapeDataString(ComicSearchText)
                + "&limit=20";

            using var response = await Http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var document = JsonDocument.Parse(
                await response.Content.ReadAsStringAsync());

            var books = document.RootElement.GetProperty("docs");

            foreach (var book in books.EnumerateArray())
            {
                string title = book.TryGetProperty("title", out var t)
                    ? t.GetString() ?? "Без названия"
                    : "Без названия";

                string author = "Неизвестен";

                if (book.TryGetProperty("author_name", out var authors)
                    && authors.ValueKind == JsonValueKind.Array
                    && authors.GetArrayLength() > 0)
                {
                    author = authors[0].GetString() ?? "Неизвестен";
                }

                string cover = string.Empty;

                if (book.TryGetProperty("cover_i", out var coverId)
                    && coverId.ValueKind == JsonValueKind.Number)
                {
                    cover = "https://covers.openlibrary.org/b/id/"
                        + coverId.GetInt32() + "-L.jpg";
                }

                ComicResults.Add(new Manga
                {
                    Title = title,
                    Author = author,
                    CoverImage = cover,
                    CoverBitmap= await LoadCoverAsync(cover),
                });
            }

            if (ComicResults.Count == 0)
                ComicSearchError = "Комиксы не найдены.";
        }
        catch (Exception ex)
        {
            ComicSearchError = "Ошибка поиска: " + ex.Message;
        }
        finally
        {
            IsComicSearching = false;
        }
    }




    [RelayCommand]
    private async Task AddComic(Manga? comic)
    {
        if (comic == null)
            return;

        bool exists = await _dbContext.MangaList
            .AnyAsync(m => m.Title == comic.Title);

        if (exists)
            return;

        await _dbContext.MangaList.AddAsync(comic);
        await _dbContext.SaveChangesAsync();

        MangaList.Add(comic);
    }

    private async Task<Avalonia.Media.Imaging.Bitmap?> LoadCoverAsync(string url)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            byte[] imageBytes = await Http.GetByteArrayAsync(url);

            using var stream = new MemoryStream(imageBytes);

            return new Avalonia.Media.Imaging.Bitmap(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки обложки: " + ex.Message);
            return null;
        }
    }
}


