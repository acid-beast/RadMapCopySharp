using System.IO;

namespace RadMapCopySharp.Core.IO;

public readonly struct StaticIndexEntry
{
    public int Lookup { get; }
    public int Length { get; }
    public int Extra { get; }

    public bool IsEmpty => Lookup < 0 || Length <= 0;

    public StaticIndexEntry(int lookup, int length, int extra)
    {
        Lookup = lookup;
        Length = length;
        Extra = extra;
    }

    public static StaticIndexEntry Empty => new(-1, -1, -1);
}

public sealed class StaticIndexFile : IDisposable
{
    private readonly FileStream _stream;
    private readonly MapProfile _profile;

    public StaticIndexFile(string filePath, MapProfile profile, FileAccess access)
    {
        _profile = profile;
        _stream = new FileStream(filePath, FileMode.OpenOrCreate, access, FileShare.ReadWrite);

        var expectedBytes = (long)(profile.Width / 8) * profile.HeightBlocks * 12;
        if (_stream.Length < expectedBytes && access != FileAccess.Read)
        {
            _stream.SetLength(expectedBytes);
            InitializeEmptyEntries(expectedBytes / 12);
        }
    }

    public StaticIndexEntry ReadEntry(int blockX, int blockY)
    {
        ValidateBlock(blockX, blockY);

        var offset = GetEntryOffset(blockX, blockY);
        _stream.Seek(offset, SeekOrigin.Begin);

        using var reader = new BinaryReader(_stream, System.Text.Encoding.Default, leaveOpen: true);
        var lookup = reader.ReadInt32();
        var length = reader.ReadInt32();
        var extra = reader.ReadInt32();

        return new StaticIndexEntry(lookup, length, extra);
    }

    public void WriteEntry(int blockX, int blockY, StaticIndexEntry entry)
    {
        ValidateBlock(blockX, blockY);

        var offset = GetEntryOffset(blockX, blockY);
        _stream.Seek(offset, SeekOrigin.Begin);

        using var writer = new BinaryWriter(_stream, System.Text.Encoding.Default, leaveOpen: true);
        writer.Write(entry.Lookup);
        writer.Write(entry.Length);
        writer.Write(entry.Extra);
    }

    private long GetEntryOffset(int blockX, int blockY)
    {
        return (long)(blockX * _profile.HeightBlocks + blockY) * 12;
    }

    private void ValidateBlock(int blockX, int blockY)
    {
        if (blockX < 0 || blockY < 0 || blockX >= _profile.Width / 8 || blockY >= _profile.HeightBlocks)
        {
            throw new ArgumentOutOfRangeException($"Block out of range: ({blockX},{blockY})");
        }
    }

    private void InitializeEmptyEntries(long entryCount)
    {
        _stream.Seek(0, SeekOrigin.Begin);
        using var writer = new BinaryWriter(_stream, System.Text.Encoding.Default, leaveOpen: true);

        for (long i = 0; i < entryCount; i++)
        {
            writer.Write(-1);
            writer.Write(-1);
            writer.Write(-1);
        }
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}

