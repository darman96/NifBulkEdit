using NifBulkEdit.Core.Dtos;

namespace NifBulkEdit.Core.Services;

public interface INifBulkTextureReplacer
{
    IEnumerable<NifTextureReplaceOp> EditTextures(List<string> filePaths, string outputDir, string find, string replace,
        bool matchCase, bool dryRun, NifTextureSlotMask? slotMask);
}