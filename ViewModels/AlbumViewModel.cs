using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using MusicStore.Avalonia.Models;
using ReactiveUI;

namespace MusicStore.Avalonia.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    private readonly Album _album;

    public AlbumViewModel(Album album)
    {
        _album = album;
    }

    public string Artist => _album.Artist;

    public string Title => _album.Title;

    private Bitmap? _cover;

    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    public async Task LoadCover(CancellationToken cancellationToken)
    {
        await using (var imageStream = await _album.LoadCoverBitmapAsync(cancellationToken))
        {
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }
        await saveBitmapCover();
    }

    public async Task LoadCover()
    {
        await using (var imageStream = await _album.LoadCoverBitmapAsync())
        {
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }
        await saveBitmapCover();
    }

    public async Task SaveToDiskAsync()
    {
        await _album.SaveAsync();

        await saveBitmapCover();
    }

    private async Task saveBitmapCover()
    {
        if (Cover != null)
        {
            var bitmap = Cover;

            await Task.Run(() =>
            {
                using (var fs = _album.SaveCoverBitmapStream())
                {
                    bitmap.Save(fs);
                }
            });
        }
    }
}
