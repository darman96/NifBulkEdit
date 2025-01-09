using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Lamar;
using NifBulkEdit.Ui.Registries;
using NifBulkEdit.Ui.ViewModels;
using NifBulkEdit.Ui.Views;

namespace NifBulkEdit.Ui;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            BindingPlugins.DataValidators.RemoveAt(0);
            
            var mainWindow = new MainWindow();
            desktop.MainWindow = mainWindow;
            
            var container = new Container(new UiRegistry(desktop));
            mainWindow.DataContext = container.GetInstance<MainWindowViewModel>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}