using System.Drawing;
using RadMapCopySharp.Core;

namespace RadMapCopySharp.Core.Rendering;

public sealed class MapPreviewState
{
    public required string SourceMapPath { get; init; }
    public required string DestinationMapPath { get; init; }
    public required MapProfile SourceProfile { get; init; }
    public required MapProfile DestinationProfile { get; init; }
    public required string RadarColPath { get; init; }
    public string RegionsXmlPath { get; init; } = string.Empty;
    public string SourceSpawnsXmlPath { get; init; } = string.Empty;
    public string DestinationSpawnsXmlPath { get; init; } = string.Empty;
    public CopyRectangle? SourceRect { get; init; }
    public Point? DestinationAnchor { get; init; }
    public string OverlayMessage { get; init; } = string.Empty;

    public bool HasOverlay => SourceRect != null && DestinationAnchor.HasValue;

    public CopyRectangle? GetDestinationRect()
    {
        if (!HasOverlay || SourceRect == null || !DestinationAnchor.HasValue)
        {
            return null;
        }

        var anchor = DestinationAnchor.Value;
        return new CopyRectangle(
            anchor.X,
            anchor.Y,
            anchor.X + SourceRect.Width - 1,
            anchor.Y + SourceRect.Height - 1);
    }
}

public sealed record CopyRectangle(int X1, int Y1, int X2, int Y2)
{
    public int Width => X2 - X1 + 1;
    public int Height => Y2 - Y1 + 1;
}
