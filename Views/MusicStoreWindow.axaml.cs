using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MusicStore.Avalonia.ViewModels;
using ReactiveUI;

namespace MusicStore.Avalonia.Views;

public partial class MusicStoreWindow : ReactiveWindow<MusicStoreViewModel>
{
    public MusicStoreWindow()
    {
        InitializeComponent();

        // This line is needed to make the previwer happy (the previewer plugin cannot handle the following line).
        if (Design.IsDesignMode) return;

        this.WhenActivated(action => action(ViewModel!.BuyCommand.Subscribe(Close)));
    }
}