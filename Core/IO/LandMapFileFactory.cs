namespace RadMapCopySharp.Core.IO;

public static class LandMapFileFactory
{
    public static ILandMapFile Open(string filePath, FileAccess access)
    {
        var ext = Path.GetExtension(filePath);
        if (ext.Equals(".uop", StringComparison.OrdinalIgnoreCase))
        {
            return new UopLandMapFile(filePath, access);
        }

        return new MulLandMapFile(filePath, access);
    }
}

