namespace RadMapCopySharp.Core.Operations;

public static class AltitudeAdjustment
{
    public static sbyte Apply(AltitudeMode mode, int z1, int z2, sbyte sourceZ, Random random)
    {
        var value = mode switch
        {
            AltitudeMode.Unchanged => sourceZ,
            AltitudeMode.FixedOffset => sourceZ + z1,
            AltitudeMode.FixedRandom => random.Next(GetMin(z1, z2), GetMax(z1, z2) + 1),
            AltitudeMode.AddRandomOffset => sourceZ + random.Next(GetMin(z1, z2), GetMax(z1, z2) + 1),
            _ => sourceZ
        };

        if (value > sbyte.MaxValue)
        {
            value = sbyte.MaxValue;
        }
        else if (value < sbyte.MinValue)
        {
            value = sbyte.MinValue;
        }

        return (sbyte)value;
    }

    private static int GetMin(int z1, int z2) => z1 <= z2 ? z1 : z2;

    private static int GetMax(int z1, int z2) => z1 <= z2 ? z2 : z1;
}
