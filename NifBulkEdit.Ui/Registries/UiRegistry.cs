using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using NifBulkEdit.Core.Registries;

namespace NifBulkEdit.Ui.Registries;

public class UiRegistry : ServiceRegistry
{
    public UiRegistry(IClassicDesktopStyleApplicationLifetime desktop)
    {
        this.Scan(scanner =>
        {
            scanner.AssemblyContainingType<UiRegistry>();
            scanner.WithDefaultConventions();
        });

        this.AddSingleton(desktop);
        this.For<Window>()
            .Use(serviceProvider 
                => serviceProvider.GetInstance<IClassicDesktopStyleApplicationLifetime>().MainWindow!);
        
        this.IncludeRegistry<CoreRegistry>();
    }
}