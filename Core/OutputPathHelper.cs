namespace RadMapCopySharp.Core;

public static class OutputPathHelper
{
    public const string CopyPrefix = "copy_";

    public static string WithCopyPrefix(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath) ?? string.Empty;
        var fileName = Path.GetFileName(filePath);
        return Path.Combine(directory, CopyPrefix + fileName);
    }

    public static string ResolveNonExistingPath(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return filePath;
        }

        var prefixed = WithCopyPrefix(filePath);
        if (!File.Exists(prefixed))
        {
            return prefixed;
        }

        var directory = Path.GetDirectoryName(filePath) ?? string.Empty;
        var baseName = Path.GetFileNameWithoutExtension(prefixed);
        var extension = Path.GetExtension(prefixed);

        var suffix = 2;
        while (true)
        {
            var candidate = Path.Combine(directory, $"{baseName}_{suffix}{extension}");
            if (!File.Exists(candidate))
            {
                return candidate;
            }
            suffix++;
        }
    }
}

