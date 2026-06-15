using System.IO;

namespace RadMapCopySharp.Core.IO;

public sealed class MulLandMapFile : ILandMapFile
{
    private readonly FileStream _stream;

    public MapProfile Profile { get; }

    public MulLandMapFile(string filePath, FileAccess access)
    {
        if (!MapProfileDetector.TryDetect(filePath, out var profile) || profile == null)
        {
            throw new InvalidOperationException($"Could not detect map profile from file: {filePath}");
        }

        Profile = profile;
        _stream = new FileStream(filePath, FileMode.Open, access, FileShare.ReadWrite);
    }

    public void ReadCell(int x, int y, out ushort tileId, out sbyte z)
    {
        ValidateCoordinates(x, y);

        var offset = GetCellOffset(x, y);
        _stream.Seek(offset, SeekOrigin.Begin);

        var lo = _stream.ReadByte();
        var hi = _stream.ReadByte();
        var zByte = _stream.ReadByte();

        if (lo < 0 || hi < 0 || zByte < 0)
        {
            throw new EndOfStreamException("Unexpected EOF while reading map cell.");
        }

        tileId = (ushort)(lo | (hi << 8));
        z = (sbyte)zByte;
    }

    public void WriteCell(int x, int y, ushort tileId, sbyte z)
    {
        ValidateCoordinates(x, y);

        var offset = GetCellOffset(x, y);
        _stream.Seek(offset, SeekOrigin.Begin);
        _stream.WriteByte((byte)(tileId & 0xFF));
        _stream.WriteByte((byte)((tileId >> 8) & 0xFF));
        _stream.WriteByte((byte)z);
    }

    public void ReadLandBlockTiles(int blockX, int blockY, Span<ushort> tileIds)
    {
        if (tileIds.Length < 64)
        {
            throw new ArgumentException("tileIds span must contain at least 64 entries.", nameof(tileIds));
        }

        ValidateBlockCoordinates(blockX, blockY);

        var blockOffset = GetBlockOffset(blockX, blockY) + 4; // skip block header
        _stream.Seek(blockOffset, SeekOrigin.Begin);

        Span<byte> buffer = stackalloc byte[192];
        var read = _stream.Read(buffer);
        if (read != 192)
        {
            throw new EndOfStreamException("Unexpected EOF while reading MUL land block.");
        }

        for (var i = 0; i < 64; i++)
        {
            var p = i * 3;
            tileIds[i] = (ushort)(buffer[p] | (buffer[p + 1] << 8));
        }
    }

    private long GetCellOffset(int x, int y)
    {
        var blockX = x / 8;
        var blockY = y / 8;
        var offX = x % 8;
        var offY = y % 8;

        return GetBlockOffset(blockX, blockY) + 4 + (offY * 8 + offX) * 3;
    }

    private long GetBlockOffset(int blockX, int blockY)
    {
        return (long)(blockX * Profile.HeightBlocks + blockY) * 196;
    }

    private void ValidateCoordinates(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Profile.Width || y >= Profile.Height)
        {
            throw new ArgumentOutOfRangeException($"Coordinates out of bounds: ({x},{y}) for {Profile}");
        }
    }

    private void ValidateBlockCoordinates(int blockX, int blockY)
    {
        if (blockX < 0 || blockY < 0 || blockX >= Profile.WidthBlocks || blockY >= Profile.HeightBlocks)
        {
            throw new ArgumentOutOfRangeException($"Block coordinates out of bounds: ({blockX},{blockY}) for {Profile}");
        }
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}

