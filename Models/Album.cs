using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace MusicStore.Avalonia.Models;

public class Album
{
    public static iTunesSearchManager _itunesSearch = new();

    public string Id { get; set; }
    public string Artist { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }
    public string ReleaseDate { get; set; }
    public int TrackCount { get; set; }
    public string Genre { get; set; }
    public string Country { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }

    public Album(string id, string artist, string title, string coverUrl,
     string releaseDate, int trackCount, string genre, string country, double price, string currency)
    {
        Id = id;
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
        ReleaseDate = releaseDate;
        TrackCount = trackCount;
        Genre = genre;
        Country = country;
        Price = price;
        Currency = currency;
    }

    public static async Task<IEnumerable<Album>> SearchAsync(string title)
    {
        var results = await _itunesSearch.GetAlbumsAsync(title);
        return results.Albums.Select(item => new Album(item.CollectionId.ToString(), item.ArtistName, item.CollectionName,
         item.ArtworkUrl100.Replace("100x100bb", "600x600bb"), item.ReleaseDate, item.TrackCount, item.PrimaryGenreName, item.Country, item.CollectionPrice, item.Country));
    }

    private static readonly HttpClient s_httpClient = new();
    private string CachePath => $"./Cache/{Artist} - {Title}";

    public async Task<Stream> LoadCoverBitmapAsync(CancellationToken cancellationToken)
    {
        if (File.Exists(CachePath + ".bmp"))
        {
            return File.OpenRead(CachePath + ".bmp");
        }
        else
        {
            var data = await s_httpClient.GetByteArrayAsync(CoverUrl, cancellationToken);
            return new MemoryStream(data);
        }
    }

    public async Task<Stream> LoadCoverBitmapAsync()
    {
        if (File.Exists(CachePath + ".bmp"))
        {
            return File.OpenRead(CachePath + ".bmp");
        }
        else
        {
            var data = await s_httpClient.GetByteArrayAsync(CoverUrl);
            return new MemoryStream(data);
        }
    }

    public async Task SaveAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        using (var fs = File.OpenWrite(CachePath))
        {
            await SaveToStreamAsync(this, fs);
        }
    }

    public Stream SaveCoverBitmapStream()
    {
        return File.OpenWrite(CachePath + ".bmp");
    }

    private static async Task SaveToStreamAsync(Album data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
    }

    public static async Task<Album> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
    }

    public static async Task<IEnumerable<Album>> LoadCachedAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        var results = new List<Album>();

        foreach (var file in Directory.EnumerateFiles("./Cache"))
        {
            if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;

            await using var fs = File.OpenRead(file);
            results.Add(await LoadFromStream(fs).ConfigureAwait(false));
        }

        return results;
    }
}
