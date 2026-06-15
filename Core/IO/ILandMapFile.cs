namespace RadMapCopySharp.Core.IO;

public interface ILandMapFile : IDisposable
{
    MapProfile Profile { get; }

    void ReadCell(int x, int y, out ushort tileId, out sbyte z);

    // Reads one 8x8 land block tile IDs in row-major order (64 entries).
    void ReadLandBlockTiles(int blockX, int blockY, Span<ushort> tileIds);

    void WriteCell(int x, int y, ushort tileId, sbyte z);
}

