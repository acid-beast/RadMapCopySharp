using RadMapCopySharp.Core.Regions;

namespace RadMapCopySharp.Core.Spawns;

public sealed class SpawnsDocument
{
    private static readonly IReadOnlyList<SpawnerOverlay> Empty = Array.Empty<SpawnerOverlay>();

    public IReadOnlyDictionary<string, IReadOnlyList<SpawnerOverlay>> ByMap { get; }

    public SpawnsDocument(IReadOnlyDictionary<string, IReadOnlyList<SpawnerOverlay>> byMap)
    {
        ByMap = byMap;
    }

    public IReadOnlyList<SpawnerOverlay> GetForProfile(MapProfile profile)
    {
        var facet = FacetNames.FromFileIndex(profile.FileIndex);
        if (facet == null)
        {
            return Empty;
        }

        return ByMap.TryGetValue(facet, out var list) ? list : Empty;
    }

    public int GetCountForProfile(MapProfile profile)
    {
        return GetForProfile(profile).Count;
    }
}
