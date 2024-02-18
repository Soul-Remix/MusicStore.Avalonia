using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MusicStore.Avalonia.ViewModels;
using ReactiveUI;

namespace MusicStore.Avalonia.Views;

public partial class AlbumDetailsWindow : ReactiveWindow<AlbumDetailsWindow>
{
    public AlbumDetailsWindow()
    {
        InitializeComponent();
    }
}