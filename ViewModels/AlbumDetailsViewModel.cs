using System;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace MusicStore.Avalonia.ViewModels;

public class AlbumDetailsViewModel : ViewModelBase
{
    private AlbumViewModel _album;
    public AlbumViewModel Album { get => _album; }

    public AlbumDetailsViewModel(AlbumViewModel album)
    {
        _album = album;
    }
}
