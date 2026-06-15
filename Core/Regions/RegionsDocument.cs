namespace RadMapCopySharp.Core.Regions;

public sealed class RegionsDocument
{
    private static readonly IReadOnlyList<MapRegionOverlay> Empty = Array.Empty<MapRegionOverlay>();

    public IReadOnlyDictionary<string, IReadOnlyList<MapRegionOverlay>> ByFacet { get; }

    public RegionsDocument(IReadOnlyDictionary<string, IReadOnlyList<MapRegionOverlay>> byFacet)
    {
        ByFacet = byFacet;
    }

    public IReadOnlyList<MapRegionOverlay> GetForProfile(MapProfile profile)
    {
        var facet = FacetNames.FromFileIndex(profile.FileIndex);
        if (facet == null)
        {
            return Empty;
        }

        return ByFacet.TryGetValue(facet, out var list) ? list : Empty;
    }

    public int GetCountForProfile(MapProfile profile)
    {
        return GetForProfile(profile).Count;
    }
}
