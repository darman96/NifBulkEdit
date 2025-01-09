using NiflySharp;
using NiflySharp.Blocks;

namespace NifBulkEdit.Core.Dtos;

public record NifTextureReplaceOp(
    string FilePath,
    NifFile File,
    INiShape Mesh,
    INiShader Shader,
    BSShaderTextureSet TextureSet,
    int TextureIndex,
    string Original,
    string Replacement);