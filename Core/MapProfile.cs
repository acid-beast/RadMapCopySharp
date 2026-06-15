namespace RadMapCopySharp.Core;

public sealed class MapProfile
{
    public string Name { get; }
    public int Width { get; }
    public int Height { get; }
    public int HeightBlocks => Height / 8;
    public int WidthBlocks => Width / 8;
    public int BlockCount => WidthBlocks * HeightBlocks;
    public long MulByteLength => (long)BlockCount * 196;
    public int FileIndex { get; }

    public MapProfile(string name, int width, int height, int fileIndex)
    {
        Name = name;
        Width = width;
        Height = height;
        FileIndex = fileIndex;
    }

    public override string ToString() => $"{Name} ({Width}x{Height})";
}

