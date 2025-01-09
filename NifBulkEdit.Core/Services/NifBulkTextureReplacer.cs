using NifBulkEdit.Core.Dtos;
using NifBulkEdit.Core.Extensions;
using NiflySharp;
using NiflySharp.Blocks;

namespace NifBulkEdit.Core.Services;

public class NifBulkTextureReplacer : INifBulkTextureReplacer
{
    public IEnumerable<NifTextureReplaceOp> EditTextures(List<string> filePaths, string outputDir, string find, string replace, bool matchCase, bool dryRun, NifTextureSlotMask? slotMask)
    {
        var replaceOps = filePaths
            .Select(path => (path, file: this.loadNif(path)))
            .Where(nif => nif.file is not null)
            .SelectMany(nif => this.buildReplaceOps(nif.file!, slotMask, nif.path, find, replace, matchCase))
            .ToList();
        
        replaceOps.ForEach(op 
            => Console.WriteLine($"{op.FilePath} - {op.Mesh.Name.String}[{op.TextureIndex}]: {op.Original} -> {op.Replacement}"));

        if (!dryRun) applyReplaceOps(replaceOps, outputDir);

        return replaceOps;
    }

    private NifFile? loadNif(string path)
    {
        var nif = new NifFile();
        var result = nif.Load(path);

        if (result == 0 && nif.Valid) return nif;

        Console.WriteLine($"Failed to load file {path}!");
        return null;
    }
    
    private List<NifTextureReplaceOp> buildReplaceOps(NifFile nif, NifTextureSlotMask? slotMask, string filePath, string find, string replace, bool matchCase)
    {
        var shapes = nif.GetShapes();
        if (shapes is null) return [];

        var replaceOps = new List<NifTextureReplaceOp>();
        foreach (var shape in shapes)
        {
            var shader = nif.GetShader(shape);
        
            if (shader is null) return [];
        
            var textureSet = nif.GetBlock<BSShaderTextureSet>(shader.TextureSetRef);
        
            if (textureSet is null) return [];

            var textures = slotMask is { } mask
                ? textureSet.Textures
                    .Select((texture, index) => (Path: texture.Content, Index: index))
                    .Where(x => mask.Get(x.Index))
                : textureSet.Textures
                    .Select((texture, index) => (Path: texture.Content, Index: index));
            
            replaceOps.AddRange(textures
                .Where(texture 
                    => string.IsNullOrWhiteSpace(find) || texture.Path.Contains(find, matchCase 
                        ? StringComparison.Ordinal 
                        : StringComparison.OrdinalIgnoreCase))
                .Select(texture 
                    => new NifTextureReplaceOp(
                        filePath,
                        nif,
                        shape,
                        shader,
                        textureSet,
                        texture.Index,
                        texture.Path,
                        string.IsNullOrWhiteSpace(find) 
                            ? replace 
                            : texture.Path.Replace(find, replace, matchCase 
                                ? StringComparison.Ordinal 
                                : StringComparison.OrdinalIgnoreCase))));
        }

        return replaceOps;
    }
    
    private void applyReplaceOps(IEnumerable<NifTextureReplaceOp> replaceOps, string outputDir)
    {
        foreach (var op in replaceOps)
        {
            var textureSet = op.File.GetBlock<BSShaderTextureSet>(op.Shader.TextureSetRef);
            if (textureSet is null) throw new InvalidOperationException("Texture set not found!");

            var texture = textureSet.Textures[op.TextureIndex];
            texture.Content = op.Replacement;
            
            var result = op.File.Save(Path.Combine(outputDir, op.FilePath.Split('/').Last()));
            if (result != 0) Console.WriteLine($"Failed to save file {op.FilePath}!");
        }
    }
}