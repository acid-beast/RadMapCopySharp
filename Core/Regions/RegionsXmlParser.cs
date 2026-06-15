using System.Xml;
using RadMapCopySharp.Core.Rendering;

namespace RadMapCopySharp.Core.Regions;

public static class RegionsXmlParser
{
    public static RegionsDocument Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new InvalidDataException("regions.xml path is empty.");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("regions.xml not found.", filePath);
        }

        var byFacet = new Dictionary<string, List<MapRegionOverlay>>(StringComparer.OrdinalIgnoreCase);

        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            DtdProcessing = DtdProcessing.Ignore
        };

        using var stream = File.OpenRead(filePath);
        using var reader = XmlReader.Create(stream, settings);

        reader.MoveToContent();
        if (!reader.IsStartElement("ServerRegions"))
        {
            throw new InvalidDataException("Invalid regions.xml: root element must be <ServerRegions>.");
        }

        if (reader.IsEmptyElement)
        {
            return new RegionsDocument(new Dictionary<string, IReadOnlyList<MapRegionOverlay>>(StringComparer.OrdinalIgnoreCase));
        }

        reader.Read();
        while (!reader.EOF)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Facet")
            {
                ParseFacet(reader, byFacet);
                continue;
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "ServerRegions")
            {
                break;
            }

            reader.Read();
        }

        var result = byFacet.ToDictionary(
            kvp => kvp.Key,
            kvp => (IReadOnlyList<MapRegionOverlay>)kvp.Value,
            StringComparer.OrdinalIgnoreCase);

        return new RegionsDocument(result);
    }

    private static void ParseFacet(XmlReader reader, Dictionary<string, List<MapRegionOverlay>> byFacet)
    {
        var facet = reader.GetAttribute("name")?.Trim();
        if (string.IsNullOrWhiteSpace(facet))
        {
            SkipCurrentElement(reader);
            return;
        }

        if (!byFacet.TryGetValue(facet, out var list))
        {
            list = new List<MapRegionOverlay>();
            byFacet[facet] = list;
        }

        if (reader.IsEmptyElement)
        {
            reader.Read();
            return;
        }

        reader.Read();
        while (!reader.EOF)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "region")
            {
                ParseRegion(reader, facet, list, parentName: null, parentType: null);
                continue;
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Facet")
            {
                reader.Read();
                break;
            }

            reader.Read();
        }
    }

    private static void ParseRegion(XmlReader reader, string facet, List<MapRegionOverlay> list, string? parentName, string? parentType)
    {
        var nameAttr = reader.GetAttribute("name")?.Trim();
        var typeAttr = reader.GetAttribute("type")?.Trim();

        var currentName = string.IsNullOrWhiteSpace(nameAttr) ? parentName ?? "(unnamed region)" : nameAttr;
        var currentType = string.IsNullOrWhiteSpace(typeAttr) ? parentType : typeAttr;

        if (reader.IsEmptyElement)
        {
            reader.Read();
            return;
        }

        reader.Read();
        while (!reader.EOF)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == "rect")
                {
                    TryAddRect(reader, facet, currentName, currentType, list);
                    continue;
                }

                if (reader.Name == "region")
                {
                    ParseRegion(reader, facet, list, currentName, currentType);
                    continue;
                }

                SkipCurrentElement(reader);
                continue;
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "region")
            {
                reader.Read();
                break;
            }

            reader.Read();
        }
    }

    private static void TryAddRect(XmlReader reader, string facet, string regionName, string? regionType, List<MapRegionOverlay> list)
    {
        var x = ParseIntAttr(reader, "x");
        var y = ParseIntAttr(reader, "y");
        var width = ParseIntAttr(reader, "width");
        var height = ParseIntAttr(reader, "height");

        if (reader.IsEmptyElement)
        {
            reader.Read();
        }
        else
        {
            SkipCurrentElement(reader);
        }

        if (!x.HasValue || !y.HasValue || !width.HasValue || !height.HasValue)
        {
            return;
        }

        if (width.Value <= 0 || height.Value <= 0)
        {
            return;
        }

        var bounds = new CopyRectangle(
            x.Value,
            y.Value,
            x.Value + width.Value - 1,
            y.Value + height.Value - 1);

        list.Add(new MapRegionOverlay
        {
            Facet = facet,
            Name = regionName,
            Type = regionType,
            Bounds = bounds
        });
    }

    private static int? ParseIntAttr(XmlReader reader, string attr)
    {
        var value = reader.GetAttribute(attr);
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return int.TryParse(value, out var parsed) ? parsed : null;
    }

    private static void SkipCurrentElement(XmlReader reader)
    {
        if (reader.IsEmptyElement)
        {
            reader.Read();
            return;
        }

        reader.Skip();
    }
}
