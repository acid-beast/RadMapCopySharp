using System.Drawing.Drawing2D;
using RadMapCopySharp.Core.Regions;
using RadMapCopySharp.Core.Rendering;

namespace RadMapCopySharp.Dialogs;

public sealed class MapPreviewPanel : Panel
{
    private const float ZoomStep = 1.12f;

    private Bitmap? _bitmap;
    private Size _mapSize = new(1, 1);
    private readonly MapViewTransform _transform = new();
    private IReadOnlyList<MapRegionOverlay>? _regions;
    private bool _showRegions;
    private CopyRectangle? _overlay;
    private Point? _crosshair;
    private Color _borderColor = Color.LimeGreen;
    private bool _isPanning;
    private Point _lastMouse;
    private bool _userAdjustedView;
    private MapRegionOverlay? _hoveredRegion;

    public event EventHandler? ViewChanged;
    public event EventHandler? RegionHoverChanged;

    public float Zoom => _transform.Zoom;

    public MapRegionOverlay? HoveredRegion => _hoveredRegion;

    public MapPreviewPanel()
    {
        DoubleBuffered = true;
        BackColor = Color.Black;
    }

    public void SetContent(Bitmap? bitmap, Size mapSize, CopyRectangle? overlay, Point? crosshair, Color borderColor, bool fitToWindow = true)
    {
        var previous = _bitmap;
        _bitmap = bitmap;
        _mapSize = mapSize;
        _overlay = overlay;
        _crosshair = crosshair;
        _borderColor = borderColor;
        previous?.Dispose();

        if (fitToWindow)
        {
            FitToWindow();
            _userAdjustedView = false;
        }

        Invalidate();
        RaiseViewChanged();
    }

    public void FitToWindow()
    {
        _transform.FitToWindow(ClientSize, _mapSize);
        Invalidate();
        RaiseViewChanged();
    }

    public void SetRegions(IReadOnlyList<MapRegionOverlay>? regions, bool visible)
    {
        _regions = regions;
        _showRegions = visible;
        Invalidate();
    }

    public bool TryScreenToMap(Point screen, out Point map)
    {
        map = Point.Empty;
        if (_mapSize.Width <= 0 || _mapSize.Height <= 0)
        {
            return false;
        }

        var mapPoint = _transform.ScreenToMap(screen, ClientSize);
        var mapX = (int)Math.Floor(mapPoint.X);
        var mapY = (int)Math.Floor(mapPoint.Y);
        if (mapX < 0 || mapY < 0 || mapX >= _mapSize.Width || mapY >= _mapSize.Height)
        {
            return false;
        }

        map = new Point(mapX, mapY);
        return true;
    }

    public bool TryHitRegion(Point screen, out MapRegionOverlay? region)
    {
        region = null;
        if (!_showRegions || _regions == null || _regions.Count == 0)
        {
            return false;
        }

        if (!TryScreenToMap(screen, out var tile))
        {
            return false;
        }

        for (var i = _regions.Count - 1; i >= 0; i--)
        {
            var candidate = _regions[i];
            var bounds = candidate.Bounds;
            if (tile.X >= bounds.X1 && tile.X <= bounds.X2 && tile.Y >= bounds.Y1 && tile.Y <= bounds.Y2)
            {
                region = candidate;
                return true;
            }
        }

        return false;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _bitmap?.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.Clear(BackColor);
        if (_bitmap == null)
        {
            return;
        }

        e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

        var state = e.Graphics.Save();
        e.Graphics.TranslateTransform(_transform.PanX, _transform.PanY);
        e.Graphics.ScaleTransform(_transform.Zoom, _transform.Zoom);
        e.Graphics.DrawImage(_bitmap, 0, 0, _mapSize.Width, _mapSize.Height);

        if (_showRegions && _regions != null)
        {
            using var fill = new SolidBrush(Color.FromArgb(40, 100, 149, 237));
            using var pen = new Pen(Color.FromArgb(180, 100, 149, 237), Math.Max(1f / _transform.Zoom, 0.5f));
            foreach (var region in _regions)
            {
                var bounds = region.Bounds;
                var regionRect = new RectangleF(bounds.X1, bounds.Y1, bounds.Width, bounds.Height);
                e.Graphics.FillRectangle(fill, regionRect);
                e.Graphics.DrawRectangle(pen, regionRect.X, regionRect.Y, regionRect.Width, regionRect.Height);
            }
        }

        if (_overlay != null)
        {
            var overlayRect = new RectangleF(_overlay.X1, _overlay.Y1, _overlay.Width, _overlay.Height);
            using var fill = new SolidBrush(Color.FromArgb(55, _borderColor));
            using var pen = new Pen(_borderColor, Math.Max(1f / _transform.Zoom, 0.5f));
            e.Graphics.FillRectangle(fill, overlayRect);
            e.Graphics.DrawRectangle(pen, overlayRect.X, overlayRect.Y, overlayRect.Width, overlayRect.Height);
        }

        if (_crosshair.HasValue)
        {
            var x = _crosshair.Value.X + 0.5f;
            var y = _crosshair.Value.Y + 0.5f;
            var length = 8f / _transform.Zoom;
            using var pen = new Pen(Color.FromArgb(220, 255, 215, 0), Math.Max(1f / _transform.Zoom, 0.5f));
            e.Graphics.DrawLine(pen, x - length, y, x + length, y);
            e.Graphics.DrawLine(pen, x, y - length, x, y + length);
        }
        e.Graphics.Restore(state);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);

        if (_bitmap == null)
        {
            return;
        }

        var factor = e.Delta > 0 ? ZoomStep : 1f / ZoomStep;
        _transform.ZoomAt(e.Location, factor, ClientSize, _mapSize);
        _userAdjustedView = true;
        Invalidate();
        RaiseViewChanged();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        _isPanning = true;
        _lastMouse = e.Location;
        Cursor = Cursors.Hand;
        ClearHoveredRegion();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (_isPanning)
        {
            var dx = e.X - _lastMouse.X;
            var dy = e.Y - _lastMouse.Y;
            _transform.PanX += dx;
            _transform.PanY += dy;
            _lastMouse = e.Location;
            _userAdjustedView = true;
            Invalidate();
            ClearHoveredRegion();
        }
        else
        {
            UpdateHoveredRegion(e.Location);
        }

        RaiseViewChanged();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);

        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        _isPanning = false;
        Cursor = Cursors.Default;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        ClearHoveredRegion();
        RaiseViewChanged();
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);
        if (!_userAdjustedView)
        {
            _transform.FitToWindow(ClientSize, _mapSize);
            Invalidate();
            RaiseViewChanged();
        }
    }

    private void RaiseViewChanged()
    {
        ViewChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateHoveredRegion(Point screen)
    {
        var previous = _hoveredRegion;
        if (!TryHitRegion(screen, out var region))
        {
            region = null;
        }

        if (ReferenceEquals(previous, region))
        {
            return;
        }

        _hoveredRegion = region;
        RegionHoverChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ClearHoveredRegion()
    {
        if (_hoveredRegion == null)
        {
            return;
        }

        _hoveredRegion = null;
        RegionHoverChanged?.Invoke(this, EventArgs.Empty);
    }
}
