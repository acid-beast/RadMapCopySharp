namespace RadMapCopySharp.Core.Validation;

public static class CoordinateValidator
{
    public static bool IsPathValid(string path)
    {
        return !string.IsNullOrWhiteSpace(path) && File.Exists(path);
    }

    public static bool ValidateRect(
        int x1,
        int y1,
        int x2,
        int y2,
        int mapWidth,
        int mapHeight,
        out string? error)
    {
        error = null;

        if (x1 < 0 || y1 < 0 || x2 < 0 || y2 < 0)
        {
            error = "Coordinates cannot be negative.";
            return false;
        }

        if (x1 > x2 || y1 > y2)
        {
            error = "Source rectangle must have x1 <= x2 and y1 <= y2.";
            return false;
        }

        if (x2 >= mapWidth || y2 >= mapHeight)
        {
            error = "Source rectangle exceeds source map bounds.";
            return false;
        }

        return true;
    }

    public static bool ValidateDestination(
        int destX,
        int destY,
        int rectWidth,
        int rectHeight,
        int mapWidth,
        int mapHeight,
        out string? error)
    {
        error = null;

        if (destX < 0 || destY < 0)
        {
            error = "Destination cannot be negative.";
            return false;
        }

        var destMaxX = destX + rectWidth - 1;
        var destMaxY = destY + rectHeight - 1;

        if (destMaxX >= mapWidth || destMaxY >= mapHeight)
        {
            error = "Destination rectangle exceeds destination map bounds.";
            return false;
        }

        return true;
    }

    public static bool TryParseInt(string text, out int value)
    {
        return int.TryParse(text, out value);
    }

    public static bool AreBlockAligned(int x1, int y1, int x2, int y2)
    {
        return (x1 % 8) == 0 && (y1 % 8) == 0 && (x2 % 8) == 0 && (y2 % 8) == 0;
    }

    public static bool ValidateBlockAligned(int x1, int y1, int x2, int y2, out string? error)
    {
        if (!AreBlockAligned(x1, y1, x2, y2))
        {
            error = "Statics copy requires source coordinates divisible by 8.";
            return false;
        }

        error = null;
        return true;
    }

    public static bool ValidateStaticsAlignment(int x1, int y1, int x2, int y2, int dx, int dy, out string? error)
    {
        if (!AreBlockAligned(x1, y1, x2, y2))
        {
            error = "Statics copy requires source coordinates divisible by 8.";
            return false;
        }

        if ((dx % 8) != 0 || (dy % 8) != 0)
        {
            error = "Statics copy requires destination coordinates divisible by 8.";
            return false;
        }

        error = null;
        return true;
    }
}

