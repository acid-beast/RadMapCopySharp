using System.Text.RegularExpressions;
using RadMapCopySharp.Core.IO;

namespace RadMapCopySharp.Core;

public static class MapProfileDetector
{
    private const int MapBlockSize = 196;
    private const long LegacyClassic6144MulBytes = 77170884;

    private static readonly CatalogEntry[] Catalog =
    {
        new(7168, 4096, 0, "Felucca (7168x4096)"),
        new(7168, 4096, 1, "Trammel / ML (7168x4096)"),
        new(6144, 4096, 0, "Felucca/Trammel (Classic 6144x4096)"),
        new(6144, 4096, 1, "Felucca/Trammel (Classic 6144x4096)"),
        new(2304, 1600, 2, "Ilshenar"),
        new(2560, 2048, 3, "Malas"),
        new(1448, 1448, 4, "Tokuno"),
        new(1280, 4096, 5, "TerMur")
    };

    private static readonly MapProfile[] Presets =
    {
        new("Felucca (7168x4096)", 7168, 4096, 0),
        new("Trammel / ML (7168x4096)", 7168, 4096, 1),
        new("Felucca/Trammel (Classic 6144x4096)", 6144, 4096, 0),
        new("Ilshenar", 2304, 1600, 2),
        new("Malas", 2560, 2048, 3),
        new("Tokuno", 1448, 1448, 4),
        new("TerMur", 1280, 4096, 5)
    };

    private static readonly Dictionary<string, MapProfile> OverrideProfiles = LoadProfileOverrides();

    public static IReadOnlyList<MapProfile> AllProfiles => Presets;

    public static bool TryDetect(string filePath, out MapProfile? profile)
    {
        return TryDetect(filePath, out profile, out _);
    }

    public static bool TryDetect(string filePath, out MapProfile? profile, out string reason)
    {
        profile = null;
        reason = string.Empty;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            reason = "Path is empty.";
            return false;
        }

        if (!File.Exists(filePath))
        {
            reason = "File does not exist.";
            return false;
        }

        var fullPath = Path.GetFullPath(filePath);
        if (OverrideProfiles.TryGetValue(fullPath, out var overridden))
        {
            profile = overridden;
            return true;
        }

        var indexHint = TryGetMapIndexHint(fullPath);
        var extension = Path.GetExtension(fullPath);

        if (extension.Equals(".mul", StringComparison.OrdinalIgnoreCase))
        {
            var bytes = new FileInfo(fullPath).Length;
            profile = DetectMul(bytes, indexHint, out reason);
            return profile != null;
        }

        if (extension.Equals(".uop", StringComparison.OrdinalIgnoreCase))
        {
            if (!UopIndexReader.TryRead(fullPath, out var indexData, out var readError) || indexData == null)
            {
                reason = readError ?? "Failed to read UOP index.";
                return false;
            }

            profile = DetectUop(indexData.LogicalLength, indexHint, out reason);
            return profile != null;
        }

        reason = $"Unsupported extension '{extension}'.";
        return false;
    }

    private static MapProfile? DetectMul(long bytes, int? indexHint, out string reason)
    {
        reason = string.Empty;

        if (bytes == LegacyClassic6144MulBytes)
        {
            var hint = indexHint ?? 0;
            return BuildProfile(6144, 4096, hint, "Felucca/Trammel (Classic 6144x4096)");
        }

        var exactMatches = Catalog
            .Where(c => ((long)c.BlockCount * MapBlockSize) == bytes)
            .ToList();

        if (exactMatches.Count == 0)
        {
            reason = $"MUL size {bytes} bytes does not match any known map dimensions.";
            return null;
        }

        return ResolveByHint(exactMatches, indexHint, out reason);
    }

    private static MapProfile? DetectUop(long logicalBytes, int? indexHint, out string reason)
    {
        reason = string.Empty;

        if (logicalBytes <= 0)
        {
            reason = "UOP logical data length is empty.";
            return null;
        }

        if ((logicalBytes % MapBlockSize) != 0)
        {
            reason = $"UOP logical data length {logicalBytes} is not aligned to {MapBlockSize}-byte blocks.";
            return null;
        }

        var blockCount = (int)(logicalBytes / MapBlockSize);

        var exactMatches = Catalog
            .Where(c => c.BlockCount == blockCount)
            .ToList();

        if (exactMatches.Count > 0)
        {
            return ResolveByHint(exactMatches, indexHint, out reason);
        }

        var tolerantMatches = Catalog
            .Where(c => Math.Abs(c.BlockCount - blockCount) <= 1)
            .ToList();

        if (tolerantMatches.Count == 0)
        {
            reason = $"UOP block count {blockCount} has no known profile match (including tolerance).";
            return null;
        }

        return ResolveByHint(tolerantMatches, indexHint, out reason);
    }

    private static MapProfile? ResolveByHint(List<CatalogEntry> candidates, int? indexHint, out string reason)
    {
        reason = string.Empty;

        if (candidates.Count == 1)
        {
            var c = candidates[0];
            return BuildProfile(c.Width, c.Height, indexHint ?? c.IndexHint, c.Name);
        }

        if (indexHint.HasValue)
        {
            var byIndex = candidates.FirstOrDefault(c => c.IndexHint == indexHint.Value);
            if (byIndex != null)
            {
                return BuildProfile(byIndex.Width, byIndex.Height, indexHint.Value, byIndex.Name);
            }
        }

        reason = "Profile detection is ambiguous; filename map index hint did not disambiguate candidates.";
        return null;
    }

    private static MapProfile BuildProfile(int width, int height, int indexHint, string name)
    {
        return new MapProfile(name, width, height, indexHint);
    }

    private static int? TryGetMapIndexHint(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var match = Regex.Match(fileName, @"map(?<index>\d+)", RegexOptions.IgnoreCase);
        if (match.Success && int.TryParse(match.Groups["index"].Value, out var index))
        {
            return index;
        }

        return null;
    }

    private static Dictionary<string, MapProfile> LoadProfileOverrides()
    {
        var result = new Dictionary<string, MapProfile>(StringComparer.OrdinalIgnoreCase);

        var settingsPath = Path.Combine(AppContext.BaseDirectory, "radmapcopy.ini");
        var overrides = SettingsStore.LoadProfileOverrides(settingsPath);

        foreach (var kvp in overrides)
        {
            var fullPath = Path.GetFullPath(kvp.Key);
            var entry = kvp.Value;
            var indexHint = TryGetMapIndexHint(fullPath) ?? 0;
            var name = string.IsNullOrWhiteSpace(entry.Name)
                ? $"{entry.Width}x{entry.Height}"
                : entry.Name;

            result[fullPath] = new MapProfile(name, entry.Width, entry.Height, indexHint);
        }

        return result;
    }

    private sealed class CatalogEntry
    {
        public int Width { get; }
        public int Height { get; }
        public int IndexHint { get; }
        public string Name { get; }
        public int BlockCount => (Width / 8) * (Height / 8);

        public CatalogEntry(int width, int height, int indexHint, string name)
        {
            Width = width;
            Height = height;
            IndexHint = indexHint;
            Name = name;
        }
    }
}

