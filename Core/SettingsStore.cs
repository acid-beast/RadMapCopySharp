using System.Text;

namespace RadMapCopySharp.Core;

public sealed class SettingsStore
{
    private readonly string _settingsPath;

    public SettingsStore(string settingsPath)
    {
        _settingsPath = settingsPath;
    }

    public AppSettings Load()
    {
        if (!File.Exists(_settingsPath))
        {
            return new AppSettings();
        }

        var dict = ParseIni(File.ReadAllLines(_settingsPath));

        return new AppSettings
        {
            SourceMapPath = Get(dict, "Paths", "SourceMapPath"),
            DestinationMapPath = Get(dict, "Paths", "DestinationMapPath"),
            RadarColPath = Get(dict, "Paths", "RadarColPath"),
            RegionsXmlPath = Get(dict, "Paths", "RegionsXmlPath"),
            SourceSpawnsXmlPath = Get(dict, "Paths", "SourceSpawnsXmlPath"),
            DestinationSpawnsXmlPath = Get(dict, "Paths", "DestinationSpawnsXmlPath"),
            SourceStaidxPath = Get(dict, "Paths", "SourceStaidxPath"),
            DestinationStaidxPath = Get(dict, "Paths", "DestinationStaidxPath"),
            SourceStaticsPath = Get(dict, "Paths", "SourceStaticsPath"),
            DestinationStaticsPath = Get(dict, "Paths", "DestinationStaticsPath")
        };
    }

    public void Save(AppSettings settings)
    {
        var existingOverrides = LoadProfileOverrides(_settingsPath);

        var sb = new StringBuilder();
        sb.AppendLine("[Paths]");
        sb.AppendLine($"SourceMapPath={settings.SourceMapPath}");
        sb.AppendLine($"DestinationMapPath={settings.DestinationMapPath}");
        sb.AppendLine($"RadarColPath={settings.RadarColPath}");
        sb.AppendLine($"RegionsXmlPath={settings.RegionsXmlPath}");
        sb.AppendLine($"SourceSpawnsXmlPath={settings.SourceSpawnsXmlPath}");
        sb.AppendLine($"DestinationSpawnsXmlPath={settings.DestinationSpawnsXmlPath}");
        sb.AppendLine($"SourceStaidxPath={settings.SourceStaidxPath}");
        sb.AppendLine($"DestinationStaidxPath={settings.DestinationStaidxPath}");
        sb.AppendLine($"SourceStaticsPath={settings.SourceStaticsPath}");
        sb.AppendLine($"DestinationStaticsPath={settings.DestinationStaticsPath}");

        if (existingOverrides.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("[ProfileOverrides]");
            foreach (var kvp in existingOverrides)
            {
                var entry = kvp.Value;
                if (string.IsNullOrWhiteSpace(entry.Name))
                {
                    sb.AppendLine($"{kvp.Key}={entry.Width},{entry.Height}");
                }
                else
                {
                    sb.AppendLine($"{kvp.Key}={entry.Width},{entry.Height},{entry.Name}");
                }
            }
        }

        File.WriteAllText(_settingsPath, sb.ToString());
    }

    public static IReadOnlyDictionary<string, ProfileOverrideEntry> LoadProfileOverrides(string iniPath)
    {
        var result = new Dictionary<string, ProfileOverrideEntry>(StringComparer.OrdinalIgnoreCase);

        if (!File.Exists(iniPath))
        {
            return result;
        }

        var lines = File.ReadAllLines(iniPath);
        var inSection = false;

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0 || line.StartsWith(";") || line.StartsWith("#"))
            {
                continue;
            }

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                inSection = string.Equals(line[1..^1].Trim(), "ProfileOverrides", StringComparison.OrdinalIgnoreCase);
                continue;
            }

            if (!inSection)
            {
                continue;
            }

            var eq = line.IndexOf('=');
            if (eq <= 0)
            {
                continue;
            }

            var path = line[..eq].Trim();
            var rhs = line[(eq + 1)..].Trim();
            var parts = rhs.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                continue;
            }

            if (!int.TryParse(parts[0], out var width) || !int.TryParse(parts[1], out var height))
            {
                continue;
            }

            var name = parts.Length >= 3 ? string.Join(',', parts.Skip(2)) : string.Empty;
            var fullPath = Path.GetFullPath(path);
            result[fullPath] = new ProfileOverrideEntry(width, height, name);
        }

        return result;
    }

    public static IReadOnlyList<SkipTilePreset> LoadSkipTilePresets(string definitionsPath)
    {
        if (!File.Exists(definitionsPath))
        {
            return Array.Empty<SkipTilePreset>();
        }

        var sections = ParseIni(File.ReadAllLines(definitionsPath));
        var presets = new List<SkipTilePreset>();

        foreach (var (sectionName, values) in sections)
        {
            if (!values.TryGetValue("Map", out var mapValue))
            {
                continue;
            }

            var set = new HashSet<ushort>();
            var tokens = mapValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var token in tokens)
            {
                if (ushort.TryParse(token, out var id))
                {
                    set.Add(id);
                }
            }

            presets.Add(new SkipTilePreset(sectionName, set));
        }

        return presets;
    }

    private static string Get(Dictionary<string, Dictionary<string, string>> dict, string section, string key)
    {
        if (dict.TryGetValue(section, out var sectionValues) && sectionValues.TryGetValue(key, out var value))
        {
            return value;
        }

        return string.Empty;
    }

    private static Dictionary<string, Dictionary<string, string>> ParseIni(IEnumerable<string> lines)
    {
        var result = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        var currentSection = string.Empty;

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0 || line.StartsWith(";") || line.StartsWith("#"))
            {
                continue;
            }

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                currentSection = line[1..^1].Trim();
                if (!result.ContainsKey(currentSection))
                {
                    result[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                continue;
            }

            var index = line.IndexOf('=');
            if (index <= 0)
            {
                continue;
            }

            var key = line[..index].Trim();
            var value = line[(index + 1)..].Trim();

            if (!result.ContainsKey(currentSection))
            {
                result[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            result[currentSection][key] = value;
        }

        return result;
    }
}

public sealed class ProfileOverrideEntry
{
    public int Width { get; }
    public int Height { get; }
    public string Name { get; }

    public ProfileOverrideEntry(int width, int height, string name)
    {
        Width = width;
        Height = height;
        Name = name;
    }
}

public sealed class AppSettings
{
    public string SourceMapPath { get; set; } = string.Empty;
    public string DestinationMapPath { get; set; } = string.Empty;
    public string RadarColPath { get; set; } = string.Empty;
    public string RegionsXmlPath { get; set; } = string.Empty;
    public string SourceSpawnsXmlPath { get; set; } = string.Empty;
    public string DestinationSpawnsXmlPath { get; set; } = string.Empty;
    public string SourceStaidxPath { get; set; } = string.Empty;
    public string DestinationStaidxPath { get; set; } = string.Empty;
    public string SourceStaticsPath { get; set; } = string.Empty;
    public string DestinationStaticsPath { get; set; } = string.Empty;
}

public sealed class SkipTilePreset
{
    public string Name { get; }
    public HashSet<ushort> TileIds { get; }

    public SkipTilePreset(string name, HashSet<ushort> tileIds)
    {
        Name = name;
        TileIds = tileIds;
    }

    public override string ToString() => Name;
}

