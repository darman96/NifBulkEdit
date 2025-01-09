using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NifBulkEdit.Core.Dtos;
using NifBulkEdit.Core.Extensions;
using NifBulkEdit.Core.Services;
using NifBulkEdit.Ui.Services;

namespace NifBulkEdit.Ui.ViewModels;

public partial class MainWindowViewModel(IApplicationService app, IFileService fileService, INifBulkTextureReplacer bulkTextureReplacer) 
    : ViewModelBase
{
    [ObservableProperty] private string find = string.Empty;
    [ObservableProperty] private string replace = string.Empty;
    [ObservableProperty] private bool matchCase;
    
    [ObservableProperty] private bool dryRun;
    [ObservableProperty] private bool specifyTextureSlots;

    [ObservableProperty] private bool slot0;
    [ObservableProperty] private bool slot1;
    [ObservableProperty] private bool slot2;
    [ObservableProperty] private bool slot3;
    [ObservableProperty] private bool slot4;
    [ObservableProperty] private bool slot5;
    [ObservableProperty] private bool slot6;
    [ObservableProperty] private bool slot7;
    [ObservableProperty] private bool slot8;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    private List<string> loadedFiles = [];
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    private string outputDirectory = string.Empty;

    [ObservableProperty] private string result = string.Empty;

    [RelayCommand]
    private async Task SelectFiles(CancellationToken cancellationToken)
    {
        try
        {
            var files = await fileService.OpenFilesAsync();
            this.LoadedFiles = files.Select(file => file.Path.AbsolutePath).ToList();
            
            Console.WriteLine("Loaded files:");
            this.LoadedFiles.ForEach(f => Console.WriteLine(f));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [RelayCommand]
    private async Task SelectOutputDirectory(CancellationToken cancellationToken)
    {
        try
        {
            var directory = await fileService.OpenFolderAsync();
            Console.WriteLine($"Output directory: {directory.Path.AbsolutePath}");
            OutputDirectory = directory.Path.AbsolutePath;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [RelayCommand(CanExecute = nameof(canStart))]
    private void Start()
    {
        var slotMask = this.SpecifyTextureSlots
            ? new NifTextureSlotMask(this.Slot0, this.Slot1, this.Slot2, this.Slot3, this.Slot4, this.Slot5, this.Slot6, this.Slot7, this.Slot8)
            : null;
            
        var ops = bulkTextureReplacer.EditTextures(this.LoadedFiles, this.OutputDirectory, this.Find, this.Replace, this.MatchCase, this.DryRun, slotMask);
        
        Result = string.Join(Environment.NewLine, ops.Select(op => $"{op.FilePath} - {op.Mesh.Name.String}[{op.TextureIndex}]: {op.Original} -> {op.Replacement}"));
    }

    private bool canStart() => this.LoadedFiles.Any() && !string.IsNullOrWhiteSpace(this.OutputDirectory);
    
    [RelayCommand]
    private void Close() => app.Shutdown();
}