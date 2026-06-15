using RadMapCopySharp.Core.Rendering;

namespace RadMapCopySharp.Core.Regions;

public sealed class MapRegionOverlay
{
    public required string Facet { get; init; }
    public required string Name { get; init; }
    public string? Type { get; init; }
    public required CopyRectangle Bounds { get; init; }
}
