namespace RadMapCopySharp.Core.Operations;

public sealed class ExtendToMLRequest
{
    public required string SourceMapPath { get; init; }
    public required string DestinationMapPath { get; init; }
    public required ushort FillTileId { get; init; }
    public required sbyte FillZ { get; init; }
}

public sealed class ExtendToMLOperation
{
    private static readonly MapProfile MlProfile = new("Mondain's Legacy", 7168, 4096, 1);

    public ExtendToMLResult Execute(ExtendToMLRequest request, Action<int, string>? progress)
    {
        if (!MapProfileDetector.TryDetect(request.SourceMapPath, out var sourceProfile) || sourceProfile == null)
        {
            throw new InvalidOperationException("Could not detect source map profile.");
        }

        if (sourceProfile.Width == MlProfile.Width && sourceProfile.Height == MlProfile.Height)
        {
            throw new InvalidOperationException("Source map is already Mondain's Legacy size (7168x4096).");
        }

        var resolvedDestinationPath = OutputPathHelper.ResolveNonExistingPath(request.DestinationMapPath);

        var createOperation = new CreateEmptyMapOperation();
        createOperation.Execute(new CreateEmptyMapRequest
        {
            MapPath = resolvedDestinationPath,
            Profile = MlProfile,
            TileId = request.FillTileId,
            Z = request.FillZ,
            CreateEmptyStatics = false
        }, (p, m) => progress?.Invoke(p / 2, m));

        var copyOperation = new MapCopyOperation();
        copyOperation.Execute(new MapCopyRequest
         {
             SourcePath = request.SourceMapPath,
             DestinationPath = resolvedDestinationPath,
             SourceX1 = 0,
             SourceY1 = 0,
             SourceX2 = sourceProfile.Width - 1,
             SourceY2 = sourceProfile.Height - 1,
             DestinationX = 0,
             DestinationY = 0,
             AltitudeMode = AltitudeMode.Unchanged,
             Z1 = 0,
             Z2 = 0,
             SkipTileIds = null
         }, (p, m) => progress?.Invoke(50 + (p / 2), m));

        progress?.Invoke(100, "Extend to ML complete.");

        return new ExtendToMLResult(request.DestinationMapPath, resolvedDestinationPath);
    }
}

public sealed class ExtendToMLResult
{
    public string RequestedDestinationPath { get; }
    public string ResolvedDestinationPath { get; }

    public ExtendToMLResult(string requestedDestinationPath, string resolvedDestinationPath)
    {
        RequestedDestinationPath = requestedDestinationPath;
        ResolvedDestinationPath = resolvedDestinationPath;
    }
}

