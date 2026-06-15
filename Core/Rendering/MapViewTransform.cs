using System.Drawing;

namespace RadMapCopySharp.Core.Rendering;

public sealed class MapViewTransform
{
    private const float MinZoom = 0.05f;
    private const float MaxZoom = 32f;

    public float Zoom { get; set; } = 1f;
    public float PanX { get; set; }
    public float PanY { get; set; }

    public void FitToWindow(Size clientSize, Size mapSize)
    {
        if (clientSize.Width <= 0 || clientSize.Height <= 0 || mapSize.Width <= 0 || mapSize.Height <= 0)
        {
            Zoom = 1f;
            PanX = 0f;
            PanY = 0f;
            return;
        }

        Zoom = Math.Clamp(
            Math.Min(clientSize.Width / (float)mapSize.Width, clientSize.Height / (float)mapSize.Height),
            MinZoom,
            MaxZoom);

        PanX = (clientSize.Width - mapSize.Width * Zoom) / 2f;
        PanY = (clientSize.Height - mapSize.Height * Zoom) / 2f;
    }

    public void ZoomAt(Point screenPoint, float factor, Size clientSize, Size mapSize)
    {
        if (mapSize.Width <= 0 || mapSize.Height <= 0 || factor <= 0f)
        {
            return;
        }

        var mapBefore = ScreenToMap(screenPoint, clientSize);
        var nextZoom = Math.Clamp(Zoom * factor, MinZoom, MaxZoom);
        Zoom = nextZoom;

        PanX = screenPoint.X - mapBefore.X * Zoom;
        PanY = screenPoint.Y - mapBefore.Y * Zoom;
    }

    public PointF ScreenToMap(Point screen, Size clientSize)
    {
        if (Zoom <= 0f)
        {
            return PointF.Empty;
        }

        return new PointF(
            (screen.X - PanX) / Zoom,
            (screen.Y - PanY) / Zoom);
    }

    public RectangleF MapRectToScreen(CopyRectangle tileRect, Size clientSize)
    {
        var left = tileRect.X1 * Zoom + PanX;
        var top = tileRect.Y1 * Zoom + PanY;
        var width = tileRect.Width * Zoom;
        var height = tileRect.Height * Zoom;
        return new RectangleF(left, top, width, height);
    }
}
