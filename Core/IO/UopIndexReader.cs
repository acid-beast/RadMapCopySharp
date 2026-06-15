namespace RadMapCopySharp.Core.IO;

public static class UopIndexReader
{
    private const int Signature = 0x50594D;

    private static readonly object Sync = new();
    private static readonly Dictionary<string, CacheEntry> Cache = new(StringComparer.OrdinalIgnoreCase);

    public static bool TryRead(string filePath, out UopIndexData? data, out string? error)
    {
        data = null;
        error = null;

        if (!File.Exists(filePath))
        {
            error = "UOP file does not exist.";
            return false;
        }

        var fullPath = Path.GetFullPath(filePath);
        var info = new FileInfo(fullPath);

        lock (Sync)
        {
            if (Cache.TryGetValue(fullPath, out var cached) && cached.Length == info.Length && cached.LastWriteUtc == info.LastWriteTimeUtc)
            {
                data = cached.Data;
                return true;
            }
        }

        try
        {
            var parsed = ReadInternal(fullPath);

            lock (Sync)
            {
                Cache[fullPath] = new CacheEntry(info.Length, info.LastWriteTimeUtc, parsed);
            }

            data = parsed;
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public static long GetLogicalDataLength(string filePath)
    {
        if (!TryRead(filePath, out var data, out var error) || data == null)
        {
            throw new InvalidOperationException(error ?? "Failed to read UOP index.");
        }

        return data.LogicalLength;
    }

    private static UopIndexData ReadInternal(string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new BinaryReader(stream);

        if (reader.ReadInt32() != Signature)
        {
            throw new InvalidOperationException("Bad UOP file signature.");
        }

        reader.ReadInt64();
        var nextBlock = reader.ReadInt64();
        reader.ReadInt32();
        var count = reader.ReadInt32();

        if (count <= 0)
        {
            throw new InvalidOperationException("UOP contains no entries.");
        }

        var pattern = Path.GetFileNameWithoutExtension(filePath).ToLowerInvariant();
        var hashToIndex = new Dictionary<ulong, int>();
        for (var i = 0; i < count; i++)
        {
            var entryName = $"build/{pattern}/{i:D8}.dat";
            var hash = UopHash.HashFileName(entryName);
            if (!hashToIndex.ContainsKey(hash))
            {
                hashToIndex.Add(hash, i);
            }
        }

        var entries = new UopIndexEntry[count];

        if (nextBlock != 0)
        {
            stream.Seek(nextBlock, SeekOrigin.Begin);
        }

        while (nextBlock != 0)
        {
            var filesCount = reader.ReadInt32();
            nextBlock = reader.ReadInt64();

            for (var i = 0; i < filesCount; i++)
            {
                var offset = reader.ReadInt64();
                var headerLength = reader.ReadInt32();
                var compressedLength = reader.ReadInt32();
                var decompressedLength = reader.ReadInt32();
                var hash = reader.ReadUInt64();
                reader.ReadUInt32();
                var flag = reader.ReadInt16();

                if (offset == 0)
                {
                    continue;
                }

                if (flag == 1)
                {
                    throw new InvalidOperationException("Compressed UOP map entries are not supported.");
                }

                if (!hashToIndex.TryGetValue(hash, out var index) || index < 0 || index >= entries.Length)
                {
                    throw new InvalidOperationException($"Unknown UOP hash entry: 0x{hash:X8}");
                }

                entries[index] = new UopIndexEntry(offset + headerLength, decompressedLength);
            }

            if (nextBlock != 0)
            {
                stream.Seek(nextBlock, SeekOrigin.Begin);
            }
        }

        long logicalLength = 0;
        foreach (var entry in entries)
        {
            if (entry.Length <= 0)
            {
                continue;
            }

            logicalLength += entry.Length;
        }

        if (logicalLength <= 0)
        {
            throw new InvalidOperationException("UOP contains no readable map data entries.");
        }

        return new UopIndexData(entries, logicalLength);
    }

    private readonly record struct CacheEntry(long Length, DateTime LastWriteUtc, UopIndexData Data);
}

public sealed class UopIndexData
{
    public IReadOnlyList<UopIndexEntry> Entries { get; }
    public long LogicalLength { get; }

    public UopIndexData(IReadOnlyList<UopIndexEntry> entries, long logicalLength)
    {
        Entries = entries;
        LogicalLength = logicalLength;
    }
}

public readonly struct UopIndexEntry
{
    public long Offset { get; }
    public int Length { get; }

    public UopIndexEntry(long offset, int length)
    {
        Offset = offset;
        Length = length;
    }
}

