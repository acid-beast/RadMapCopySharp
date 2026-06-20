using RadMapCopySharp.Core.Rendering;

namespace RadMapCopySharp.Core.Spawns;

public sealed class SpawnerOverlay
{
    public required string Map { get; init; }
    public required string Name { get; init; }
    public required CopyRectangle Bounds { get; init; }
    public required int CentreX { get; init; }
    public required int CentreY { get; init; }
    public int Range { get; init; }
    public int MaxCount { get; init; }
    public int MinDelay { get; init; }
    public int MaxDelay { get; init; }
    public bool DelayInSec { get; init; }
    public required IReadOnlyList<string> Creatures { get; init; }
}
