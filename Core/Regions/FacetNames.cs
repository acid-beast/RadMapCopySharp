namespace RadMapCopySharp.Core.Regions;

public static class FacetNames
{
    public static string? FromFileIndex(int fileIndex)
    {
        return fileIndex switch
        {
            0 => "Felucca",
            1 => "Trammel",
            2 => "Ilshenar",
            3 => "Malas",
            4 => "Tokuno",
            5 => "TerMur",
            _ => null
        };
    }
}
