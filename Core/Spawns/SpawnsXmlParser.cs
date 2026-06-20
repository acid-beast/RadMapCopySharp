using System.Globalization;
using System.Xml;
using RadMapCopySharp.Core.Rendering;

namespace RadMapCopySharp.Core.Spawns;

public static class SpawnsXmlParser
{
    public static SpawnsDocument Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new InvalidDataException("spawns xml path is empty.");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("spawns xml not found.", filePath);
        }

        var byMap = new Dictionary<string, List<SpawnerOverlay>>(StringComparer.OrdinalIgnoreCase);

        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            DtdProcessing = DtdProcessing.Ignore
        };

        using var stream = File.OpenRead(filePath);
        using var reader = XmlReader.Create(stream, settings);

        reader.MoveToContent();
        if (!reader.IsStartElement("Spawns"))
        {
            throw new InvalidDataException("Invalid spawns xml: root element must be <Spawns>.");
        }

        if (reader.IsEmptyElement)
        {
            return new SpawnsDocument(new Dictionary<string, IReadOnlyList<SpawnerOverlay>>(StringComparer.OrdinalIgnoreCase));
        }

        reader.Read();
        while (!reader.EOF)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Points")
            {
                ParsePoints(reader, byMap);
                continue;
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Spawns")
            {
                break;
            }

            reader.Read();
        }

        var result = byMap.ToDictionary(
            kvp => kvp.Key,
            kvp => (IReadOnlyList<SpawnerOverlay>)kvp.Value,
            StringComparer.OrdinalIgnoreCase);

        return new SpawnsDocument(result);
    }

    private static void ParsePoints(XmlReader reader, Dictionary<string, List<SpawnerOverlay>> byMap)
    {
        if (reader.IsEmptyElement)
        {
            reader.Read();
            return;
        }

        string? map = null;
        var name = "(unnamed spawner)";
        int x = 0, y = 0, width = 0, height = 0;
        int centreX = 0, centreY = 0;
        int range = 0, maxCount = 0, minDelay = 0, maxDelay = 0;
        var delayInSec = false;
        var creatures = new List<string>();

        reader.Read();
        while (!reader.EOF)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                var element = reader.Name;
                var value = ReadElementText(reader);

                switch (element)
                {
                    case "Map":
                        map = value.Trim();
                        break;
                    case "Name":
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            name = value.Trim();
                        }
                        break;
                    case "X":
                        x = ParseInt(value);
                        break;
                    case "Y":
                        y = ParseInt(value);
                        break;
                    case "Width":
                        width = ParseInt(value);
                        break;
                    case "Height":
                        height = ParseInt(value);
                        break;
                    case "CentreX":
                        centreX = ParseInt(value);
                        break;
                    case "CentreY":
                        centreY = ParseInt(value);
                        break;
                    case "Range":
                        range = ParseInt(value);
                        break;
                    case "MaxCount":
                        maxCount = ParseInt(value);
                        break;
                    case "MinDelay":
                        minDelay = ParseInt(value);
                        break;
                    case "MaxDelay":
                        maxDelay = ParseInt(value);
                        break;
                    case "DelayInSec":
                        delayInSec = ParseBool(value);
                        break;
                    case "Objects2":
                        AddCreature(creatures, value);
                        break;
                }

                continue;
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Points")
            {
                reader.Read();
                break;
            }

            reader.Read();
        }

        if (string.IsNullOrWhiteSpace(map) || width <= 0 || height <= 0)
        {
            return;
        }

        if (!byMap.TryGetValue(map, out var list))
        {
            list = new List<SpawnerOverlay>();
            byMap[map] = list;
        }

        list.Add(new SpawnerOverlay
        {
            Map = map,
            Name = name,
            Bounds = new CopyRectangle(x, y, x + width - 1, y + height - 1),
            CentreX = centreX,
            CentreY = centreY,
            Range = range,
            MaxCount = maxCount,
            MinDelay = minDelay,
            MaxDelay = maxDelay,
            DelayInSec = delayInSec,
            Creatures = creatures
        });
    }

    private static string ReadElementText(XmlReader reader)
    {
        if (reader.IsEmptyElement)
        {
            reader.Read();
            return string.Empty;
        }

        return reader.ReadElementContentAsString();
    }

    private static void AddCreature(List<string> creatures, string objects2)
    {
        if (string.IsNullOrWhiteSpace(objects2))
        {
            return;
        }

        var token = objects2.Split('/', ':')[0].Trim();
        if (token.Length == 0 || creatures.Contains(token, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        creatures.Add(token);
    }

    private static int ParseInt(string value)
    {
        return int.TryParse(value?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : 0;
    }

    private static bool ParseBool(string value)
    {
        return bool.TryParse(value?.Trim(), out var parsed) && parsed;
    }
}
