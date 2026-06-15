using System.Drawing;

namespace RadMapCopySharp.Core.IO;

public sealed class RadarColorTable
{
    public const int EntryCount = 65536;
    public const int ExpectedByteLength = EntryCount * 2;

    private readonly Color[] _landColors;

    private RadarColorTable(Color[] landColors)
    {
        _landColors = landColors;
    }

    public static RadarColorTable Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new InvalidOperationException("radarcol path is empty.");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("radarcol.mul was not found.", filePath);
        }

        var bytes = File.ReadAllBytes(filePath);
        if (bytes.Length < ExpectedByteLength)
        {
            throw new InvalidDataException(
                $"radarcol.mul is too small ({bytes.Length} bytes; need at least {ExpectedByteLength}).");
        }

        var colors = new Color[EntryCount];
        for (var i = 0; i < EntryCount; i++)
        {
            var colorValue = BitConverter.ToUInt16(bytes, i * 2);
            colors[i] = FromUoColor(colorValue);
        }

        return new RadarColorTable(colors);
    }

    public Color GetLandColor(ushort tileId)
    {
        return _landColors[tileId];
    }

    private static Color FromUoColor(ushort value)
    {
        var r = ((value >> 10) & 0x1F) * 255 / 31;
        var g = ((value >> 5) & 0x1F) * 255 / 31;
        var b = (value & 0x1F) * 255 / 31;
        return Color.FromArgb(r, g, b);
    }
}
