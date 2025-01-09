using Avalonia.Controls.ApplicationLifetimes;

namespace NifBulkEdit.Ui.Services;

public class ApplicationService(IClassicDesktopStyleApplicationLifetime desktop) : IApplicationService
{
    public void Shutdown() => desktop.Shutdown();
}