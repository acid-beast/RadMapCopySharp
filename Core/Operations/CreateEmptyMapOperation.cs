using RadMapCopySharp.Core.IO;

namespace RadMapCopySharp.Core.Operations;

public sealed class CreateEmptyMapRequest
{
    public required string MapPath { get; init; }
    public required MapProfile Profile { get; init; }
    public required ushort TileId { get; init; }
    public required sbyte Z { get; init; }
    public required bool CreateEmptyStatics { get; init; }
    public string? StaidxPath { get; init; }
    public string? StaticsPath { get; init; }
}

public sealed class CreateEmptyMapOperation
{
    public CreateEmptyMapResult Execute(CreateEmptyMapRequest request, Action<int, string>? progress)
    {
        var resolvedMapPath = OutputPathHelper.ResolveNonExistingPath(request.MapPath);
        var resolvedStaidxPath = request.CreateEmptyStatics && !string.IsNullOrWhiteSpace(request.StaidxPath)
            ? OutputPathHelper.ResolveNonExistingPath(request.StaidxPath)
            : null;
        var resolvedStaticsPath = request.CreateEmptyStatics && !string.IsNullOrWhiteSpace(request.StaticsPath)
            ? OutputPathHelper.ResolveNonExistingPath(request.StaticsPath)
            : null;

        var blocksX = request.Profile.Width / 8;
        var blocksY = request.Profile.Height / 8;
        var totalBlocks = blocksX * blocksY;

        using (var stream = new FileStream(resolvedMapPath, FileMode.Create, FileAccess.Write, FileShare.Read))
        using (var writer = new BinaryWriter(stream))
        {
            for (var i = 0; i < totalBlocks; i++)
            {
                writer.Write(0u);
                for (var cell = 0; cell < 64; cell++)
                {
                    writer.Write(request.TileId);
                    writer.Write(request.Z);
                }

                if (i % 2048 == 0)
                {
                    var percent = (int)((i / (double)totalBlocks) * 100.0);
                    progress?.Invoke(percent, $"Writing map blocks... {i}/{totalBlocks}");
                }
            }
        }

        if (request.CreateEmptyStatics)
        {
            if (string.IsNullOrWhiteSpace(resolvedStaidxPath) || string.IsNullOrWhiteSpace(resolvedStaticsPath))
            {
                throw new InvalidOperationException("Staidx and statics paths are required when CreateEmptyStatics is enabled.");
            }

            using (var idx = new StaticIndexFile(resolvedStaidxPath, request.Profile, FileAccess.ReadWrite))
            {
                // Constructor ensures file is fully initialized with empty entries.
            }

            using (var sta = new FileStream(resolvedStaticsPath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                sta.SetLength(0);
            }
        }

        progress?.Invoke(100, "Create map complete.");

        return new CreateEmptyMapResult(
            request.MapPath,
            resolvedMapPath,
            request.CreateEmptyStatics ? request.StaidxPath : null,
            resolvedStaidxPath,
            request.CreateEmptyStatics ? request.StaticsPath : null,
            resolvedStaticsPath);
    }
}

public sealed class CreateEmptyMapResult
{
    public string RequestedMapPath { get; }
    public string ResolvedMapPath { get; }
    public string? RequestedStaidxPath { get; }
    public string? ResolvedStaidxPath { get; }
    public string? RequestedStaticsPath { get; }
    public string? ResolvedStaticsPath { get; }

    public CreateEmptyMapResult(
        string requestedMapPath,
        string resolvedMapPath,
        string? requestedStaidxPath,
        string? resolvedStaidxPath,
        string? requestedStaticsPath,
        string? resolvedStaticsPath)
    {
        RequestedMapPath = requestedMapPath;
        ResolvedMapPath = resolvedMapPath;
        RequestedStaidxPath = requestedStaidxPath;
        ResolvedStaidxPath = resolvedStaidxPath;
        RequestedStaticsPath = requestedStaticsPath;
        ResolvedStaticsPath = resolvedStaticsPath;
    }
}

