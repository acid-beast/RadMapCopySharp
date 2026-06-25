using RadMapCopySharp.Core.IO;
using RadMapCopySharp.Core.Validation;

namespace RadMapCopySharp.Core.Operations;

public enum AltitudeMode
{
    Unchanged = 0,
    FixedRandom = 1,
    AddRandomOffset = 2,
    FixedOffset = 3
}

public sealed class MapCopyRequest
{
    public required string SourcePath { get; init; }
    public required string DestinationPath { get; init; }
    public required int SourceX1 { get; init; }
    public required int SourceY1 { get; init; }
    public required int SourceX2 { get; init; }
    public required int SourceY2 { get; init; }
    public required int DestinationX { get; init; }
    public required int DestinationY { get; init; }
    public required AltitudeMode AltitudeMode { get; init; }
    public required int Z1 { get; init; }
    public required int Z2 { get; init; }
    public HashSet<ushort>? SkipTileIds { get; init; }
}

public sealed class MapCopyOperation
{
    public void Execute(MapCopyRequest request, Action<int, string>? progress)
    {
        using var source = LandMapFileFactory.Open(request.SourcePath, FileAccess.Read);
        using var destination = LandMapFileFactory.Open(request.DestinationPath, FileAccess.ReadWrite);

        var rectWidth = request.SourceX2 - request.SourceX1 + 1;
        var rectHeight = request.SourceY2 - request.SourceY1 + 1;

        if (!CoordinateValidator.ValidateRect(
                request.SourceX1,
                request.SourceY1,
                request.SourceX2,
                request.SourceY2,
                source.Profile.Width,
                source.Profile.Height,
                out var sourceError))
        {
            throw new InvalidOperationException(sourceError);
        }

        if (!CoordinateValidator.ValidateDestination(
                request.DestinationX,
                request.DestinationY,
                rectWidth,
                rectHeight,
                destination.Profile.Width,
                destination.Profile.Height,
                out var destinationError))
        {
            throw new InvalidOperationException(destinationError);
        }

        var random = new Random();
        var total = rectWidth * rectHeight;
        var done = 0;

        for (var sx = request.SourceX1; sx <= request.SourceX2; sx++)
        {
            var dx = request.DestinationX + (sx - request.SourceX1);

            for (var sy = request.SourceY1; sy <= request.SourceY2; sy++)
            {
                var dy = request.DestinationY + (sy - request.SourceY1);

                source.ReadCell(sx, sy, out var tileId, out var sourceZ);

                if (request.SkipTileIds != null && request.SkipTileIds.Contains(tileId))
                {
                    done++;
                    continue;
                }

                var finalZ = AltitudeAdjustment.Apply(request.AltitudeMode, request.Z1, request.Z2, sourceZ, random);
                destination.WriteCell(dx, dy, tileId, finalZ);

                done++;
                if (done % 2048 == 0)
                {
                    var percent = (int)((done / (double)total) * 100.0);
                    progress?.Invoke(percent, $"Copying map cells... {done}/{total}");
                }
            }
        }

        progress?.Invoke(100, "Map copy complete.");
    }
}

