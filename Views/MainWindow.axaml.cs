using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MusicStore.Avalonia.ViewModels;
using ReactiveUI;

namespace MusicStore.Avalonia.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(action => action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        this.WhenActivated(action => action(ViewModel!.ShowDetailsDialog.RegisterHandler(DoShowAlbumDialogAsync)));
    }

    private async Task DoShowDialogAsync(InteractionContext<MusicStoreViewModel, AlbumViewModel?> interaction)
    {
        var dialog = new MusicStoreWindow
        {
            DataContext = interaction.Input
        };

        var result = await dialog.ShowDialog<AlbumViewModel?>(this);
        interaction.SetOutput(result);
    }

    private async Task DoShowAlbumDialogAsync(InteractionContext<AlbumDetailsViewModel, AlbumViewModel?> interaction)
    {
        var dialog = new AlbumDetailsWindow
        {
            DataContext = interaction.Input
        };

        var result = await dialog.ShowDialog<AlbumViewModel?>(this);
        interaction.SetOutput(result);
    }
}