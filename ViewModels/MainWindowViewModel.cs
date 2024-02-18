using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using System.Reactive.Concurrency;
using MusicStore.Avalonia.Models;
using ReactiveUI;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Reactive;
using MusicStore.Avalonia.Views;

namespace MusicStore.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand BuyMusicCommand { get; }
    public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }
    public ObservableCollection<AlbumViewModel> Albums { get; } = [];

    private AlbumViewModel? _selectedAlbum;
    public AlbumViewModel? SelectedAlbum
    {
        get => _selectedAlbum;
        set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
    }

    public Interaction<AlbumDetailsViewModel, AlbumViewModel?> ShowDetailsDialog { get; }
    public ICommand DetailsCommand { get; }

    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
        ShowDetailsDialog = new Interaction<AlbumDetailsViewModel, AlbumViewModel?>();

        BuyMusicCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var store = new MusicStoreViewModel();
            var result = await ShowDialog.Handle(store);
            if (result != null)
            {
                Albums.Add(result);
                await result.SaveToDiskAsync();
                result?.LoadCover();
            }
        });

        DetailsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var store = new AlbumDetailsViewModel(SelectedAlbum!);
            var result = await ShowDetailsDialog.Handle(store);
        });

        RxApp.MainThreadScheduler.Schedule(LoadAlbums);
    }

    private async void LoadAlbums()
    {
        try
        {
            var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x));

            foreach (var album in albums)
            {
                Albums.Add(album);
            }

            foreach (var album in Albums.ToList())
            {
                await album.LoadCover();
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}
