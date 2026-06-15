using RadMapCopySharp.Core.IO;

namespace RadMapCopySharp.Core.Operations;

public sealed class StaticsCopyRequest
{
    public required string SourceStaidxPath { get; init; }
    public required string SourceStaticsPath { get; init; }
    public required string DestinationStaidxPath { get; init; }
    public required string DestinationStaticsPath { get; init; }
    public required MapProfile SourceProfile { get; init; }
    public required MapProfile DestinationProfile { get; init; }
    public required int SourceX1 { get; init; }
    public required int SourceY1 { get; init; }
    public required int SourceX2 { get; init; }
    public required int SourceY2 { get; init; }
    public required int DestinationX { get; init; }
    public required int DestinationY { get; init; }
    public required AltitudeMode AltitudeMode { get; init; }
    public required int Z1 { get; init; }
    public required int Z2 { get; init; }
}

public sealed class StaticsCopyOperation
{
    public void Execute(StaticsCopyRequest request, Action<int, string>? progress)
    {
        EnsureBlockAligned(request.SourceX1, request.SourceY1, request.SourceX2, request.SourceY2, request.DestinationX, request.DestinationY);

        using var sourceIdx = new StaticIndexFile(request.SourceStaidxPath, request.SourceProfile, FileAccess.Read);
        using var sourceSta = new StaticsFile(request.SourceStaticsPath, FileAccess.Read);
        using var destinationIdx = new StaticIndexFile(request.DestinationStaidxPath, request.DestinationProfile, FileAccess.ReadWrite);
        using var destinationSta = new StaticsFile(request.DestinationStaticsPath, FileAccess.ReadWrite);

        var sourceBlockX1 = request.SourceX1 / 8;
        var sourceBlockY1 = request.SourceY1 / 8;
        var sourceBlockX2 = request.SourceX2 / 8;
        var sourceBlockY2 = request.SourceY2 / 8;

        var totalBlocks = (sourceBlockX2 - sourceBlockX1 + 1) * (sourceBlockY2 - sourceBlockY1 + 1);
        var done = 0;
        var random = new Random();

        for (var sbx = sourceBlockX1; sbx <= sourceBlockX2; sbx++)
        {
            var dbx = request.DestinationX / 8 + (sbx - sourceBlockX1);
            for (var sby = sourceBlockY1; sby <= sourceBlockY2; sby++)
            {
                var dby = request.DestinationY / 8 + (sby - sourceBlockY1);

                var entry = sourceIdx.ReadEntry(sbx, sby);
                var records = sourceSta.ReadBlock(entry);

                if (records.Count > 0 && request.AltitudeMode != AltitudeMode.Unchanged)
                {
                    for (var i = 0; i < records.Count; i++)
                    {
                        var z = records[i].Z;
                        var adjusted = request.AltitudeMode == AltitudeMode.FixedRandom
                            ? random.Next(Math.Min(request.Z1, request.Z2), Math.Max(request.Z1, request.Z2) + 1)
                            : z + random.Next(Math.Min(request.Z1, request.Z2), Math.Max(request.Z1, request.Z2) + 1);

                        adjusted = Math.Clamp(adjusted, sbyte.MinValue, sbyte.MaxValue);
                        records[i] = records[i].WithZ((sbyte)adjusted);
                    }
                }

                var newEntry = destinationSta.AppendBlock(records);
                destinationIdx.WriteEntry(dbx, dby, newEntry);

                done++;
                if (done % 128 == 0)
                {
                    var percent = (int)((done / (double)totalBlocks) * 100.0);
                    progress?.Invoke(percent, $"Copying static blocks... {done}/{totalBlocks}");
                }
            }
        }

        progress?.Invoke(100, "Statics copy complete.");
    }

    private static void EnsureBlockAligned(int sx1, int sy1, int sx2, int sy2, int dx, int dy)
    {
        if ((sx1 % 8) != 0 || (sy1 % 8) != 0 || (sx2 % 8) != 0 || (sy2 % 8) != 0 || (dx % 8) != 0 || (dy % 8) != 0)
        {
            throw new InvalidOperationException("Statics copy requires coordinates divisible by 8.");
        }
    }
}

