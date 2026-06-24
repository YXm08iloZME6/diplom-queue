using System.ComponentModel;
using Avalonia.Controls;
using QueueClient.ViewModels;

namespace QueueClient.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.PropertyChanged += OnVmPropertyChanged;
            ApplyFullScreen(vm.IsFullScreen);
        }
    }

    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.IsFullScreen)
            && DataContext is MainWindowViewModel vm)
        {
            ApplyFullScreen(vm.IsFullScreen);
        }
    }

    private void ApplyFullScreen(bool fullScreen)
    {
        // Терминал и табло — на весь экран без рамок; рабочие места — обычное окно.
        if (fullScreen)
        {
            SystemDecorations = SystemDecorations.None;
            WindowState = WindowState.FullScreen;
        }
        else
        {
            SystemDecorations = SystemDecorations.Full;
            WindowState = WindowState.Normal;
        }
    }
}
