using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace NifBulkEdit.Ui.Services;

public interface IFileService
{
    Task<IEnumerable<IStorageFile>> OpenFilesAsync();
    Task<IStorageFolder?> OpenFolderAsync();
}