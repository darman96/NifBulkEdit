using Lamar;

namespace NifBulkEdit.Core.Registries;

public class CoreRegistry : ServiceRegistry
{
    public CoreRegistry()
    {
        this.Scan(scanner =>
        {
            scanner.AssemblyContainingType<CoreRegistry>();
            scanner.WithDefaultConventions();
        });
    }
}