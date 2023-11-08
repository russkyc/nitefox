using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Threading;
using WebViewCore.Events;

namespace Nitefox.Client.Avalonia;

public partial class NitefoxWindow : Window, INotifyPropertyChanged
{
    public NitefoxWindow()
    {
        DataContext = this;
        InitializeComponent();
    }

    private async void BlazorWebView_OnNavigationCompleted(object? sender, WebViewUrlLoadedEventArg e)
    {
        await Dispatcher.UIThread.InvokeAsync(() => BlazorWebView.IsVisible = true);
    }
}