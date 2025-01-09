using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace NifBulkEdit.Ui.Services;

public class FileService(Window window) : IFileService
{
    private static FilePickerFileType nifFileType = new("Nif File")
    {
        Patterns = ["*.nif"]
    };
    
    public async Task<IEnumerable<IStorageFile>> OpenFilesAsync()
    {
        return await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Nif files to edit",
            AllowMultiple = true,
            FileTypeFilter = [nifFileType]
        });
    }

    public async Task<IStorageFolder?> OpenFolderAsync()
    {
        var result = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select output directory",
            AllowMultiple = false
        });
        
        return result.FirstOrDefault();
    }
    
}