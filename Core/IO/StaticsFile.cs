using System.IO;

namespace RadMapCopySharp.Core.IO;

public readonly struct StaticRecord
{
    public ushort ItemId { get; }
    public byte X { get; }
    public byte Y { get; }
    public sbyte Z { get; }
    public ushort Hue { get; }

    public StaticRecord(ushort itemId, byte x, byte y, sbyte z, ushort hue)
    {
        ItemId = itemId;
        X = x;
        Y = y;
        Z = z;
        Hue = hue;
    }

    public StaticRecord WithZ(sbyte newZ) => new(ItemId, X, Y, newZ, Hue);
}

public sealed class StaticsFile : IDisposable
{
    private readonly FileStream _stream;

    public StaticsFile(string filePath, FileAccess access)
    {
        _stream = new FileStream(filePath, FileMode.OpenOrCreate, access, FileShare.ReadWrite);
    }

    public List<StaticRecord> ReadBlock(StaticIndexEntry entry)
    {
        if (entry.IsEmpty)
        {
            return new List<StaticRecord>();
        }

        if (entry.Lookup < 0 || entry.Length < 0)
        {
            throw new InvalidDataException("Invalid static index entry.");
        }

        _stream.Seek(entry.Lookup, SeekOrigin.Begin);
        var count = entry.Length / 7;
        var list = new List<StaticRecord>(count);

        using var reader = new BinaryReader(_stream, System.Text.Encoding.Default, leaveOpen: true);
        for (var i = 0; i < count; i++)
        {
            var itemId = reader.ReadUInt16();
            var x = reader.ReadByte();
            var y = reader.ReadByte();
            var z = reader.ReadSByte();
            var hue = reader.ReadUInt16();
            list.Add(new StaticRecord(itemId, x, y, z, hue));
        }

        return list;
    }

    public StaticIndexEntry AppendBlock(IReadOnlyList<StaticRecord> records)
    {
        if (records.Count == 0)
        {
            return StaticIndexEntry.Empty;
        }

        _stream.Seek(0, SeekOrigin.End);
        var start = checked((int)_stream.Position);

        using var writer = new BinaryWriter(_stream, System.Text.Encoding.Default, leaveOpen: true);
        foreach (var record in records)
        {
            writer.Write(record.ItemId);
            writer.Write(record.X);
            writer.Write(record.Y);
            writer.Write(record.Z);
            writer.Write(record.Hue);
        }

        var length = records.Count * 7;
        return new StaticIndexEntry(start, length, -1);
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}

