using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace MusicStore.Avalonia.Models;

public class Album
{
    public static iTunesSearchManager _itunesSearch = new();

    public string Artist { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }

    public Album(string artist, string title, string coverUrl)
    {
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
    }

    public static async Task<IEnumerable<Album>> SearchAsync(string title)
    {
        var results = await _itunesSearch.GetAlbumsAsync(title);
        return results.Albums.Select(item => new Album(item.ArtistName, item.CollectionName,
         item.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }

    private static readonly HttpClient s_httpClient = new();
    private string CachePath => $"./Cache/{Artist} - {Title}";

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
}
