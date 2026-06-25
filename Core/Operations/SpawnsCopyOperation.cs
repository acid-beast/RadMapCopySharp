using System.Globalization;
using System.Xml.Linq;
using RadMapCopySharp.Core.Regions;
using RadMapCopySharp.Core.Validation;

namespace RadMapCopySharp.Core.Operations;

public sealed class SpawnsCopyRequest
{
    public required string SourceSpawnsPath { get; init; }
    public required string DestinationSpawnsPath { get; init; }
    public required MapProfile DestinationProfile { get; init; }
    public required int SourceX1 { get; init; }
    public required int SourceY1 { get; init; }
    public required int SourceX2 { get; init; }
    public required int SourceY2 { get; init; }
    public required int DestinationX { get; init; }
    public required int DestinationY { get; init; }
    public required AltitudeMode AltitudeMode { get; init; }
    public required int Z1 { get; init; }
    public required int Z2 { get; init; }
}

public sealed class SpawnsCopyResult
{
    public string DestinationSpawnsPath { get; }
    public int CopiedCount { get; }

    public SpawnsCopyResult(string destinationSpawnsPath, int copiedCount)
    {
        DestinationSpawnsPath = destinationSpawnsPath;
        CopiedCount = copiedCount;
    }
}

public sealed class SpawnsCopyOperation
{
    public SpawnsCopyResult Execute(SpawnsCopyRequest request, Action<int, string>? progress)
    {
        if (string.IsNullOrWhiteSpace(request.SourceSpawnsPath) || !File.Exists(request.SourceSpawnsPath))
        {
            throw new InvalidOperationException("Source spawner xml is required and must exist.");
        }

        ValidateRequest(request);

        var destinationFacet = FacetNames.FromFileIndex(request.DestinationProfile.FileIndex);
        if (string.IsNullOrWhiteSpace(destinationFacet))
        {
            throw new InvalidOperationException("Could not determine destination facet name for spawner copy.");
        }

        var sourceDocument = XDocument.Load(request.SourceSpawnsPath, LoadOptions.PreserveWhitespace);
        if (sourceDocument.Root?.Name != "Spawns")
        {
            throw new InvalidDataException("Invalid source spawns xml: root element must be <Spawns>.");
        }

        var destinationDocument = LoadDestinationDocument(request.DestinationSpawnsPath);
        var destinationRoot = destinationDocument.Root!;

        var offsetX = request.DestinationX - request.SourceX1;
        var offsetY = request.DestinationY - request.SourceY1;
        var random = new Random();
        var sourcePoints = sourceDocument.Root.Elements("Points").ToList();
        var total = Math.Max(sourcePoints.Count, 1);
        var copied = 0;

        for (var i = 0; i < sourcePoints.Count; i++)
        {
            var points = sourcePoints[i];
            var x = ParseInt(points.Element("X")?.Value);
            var y = ParseInt(points.Element("Y")?.Value);
            var centreX = ParseInt(points.Element("CentreX")?.Value);
            var centreY = ParseInt(points.Element("CentreY")?.Value);
            if (!CentreInSourceRect(request, centreX, centreY))
            {
                ReportProgress(progress, i + 1, total, copied, "Scanning spawners");
                continue;
            }

            var clone = new XElement(points);
            SetInt(clone, "X", x + offsetX);
            SetInt(clone, "Y", y + offsetY);
            SetInt(clone, "CentreX", ParseInt(clone.Element("CentreX")?.Value) + offsetX);
            SetInt(clone, "CentreY", ParseInt(clone.Element("CentreY")?.Value) + offsetY);

            var centreZElement = clone.Element("CentreZ");
            if (centreZElement != null)
            {
                var centreZ = ParseInt(centreZElement.Value);
                var adjustedZ = AltitudeAdjustment.Apply(
                    request.AltitudeMode,
                    request.Z1,
                    request.Z2,
                    (sbyte)Math.Clamp(centreZ, sbyte.MinValue, sbyte.MaxValue),
                    random);
                SetInt(clone, "CentreZ", adjustedZ);
            }

            SetValue(clone, "Map", destinationFacet);
            SetValue(clone, "UniqueId", Guid.NewGuid().ToString());

            destinationRoot.Add(clone);
            copied++;
            ReportProgress(progress, i + 1, total, copied, "Copying spawners");
        }

        SaveDestinationDocument(request.DestinationSpawnsPath, destinationDocument);
        progress?.Invoke(100, $"Spawner copy complete. Copied {copied} spawners.");
        return new SpawnsCopyResult(request.DestinationSpawnsPath, copied);
    }

    private static void ValidateRequest(SpawnsCopyRequest request)
    {
        if (!CoordinateValidator.ValidateRect(
                request.SourceX1,
                request.SourceY1,
                request.SourceX2,
                request.SourceY2,
                int.MaxValue,
                int.MaxValue,
                out var sourceError))
        {
            throw new InvalidOperationException(sourceError ?? "Source spawner rectangle is invalid.");
        }

        var width = request.SourceX2 - request.SourceX1 + 1;
        var height = request.SourceY2 - request.SourceY1 + 1;
        if (!CoordinateValidator.ValidateDestination(
                request.DestinationX,
                request.DestinationY,
                width,
                height,
                request.DestinationProfile.Width,
                request.DestinationProfile.Height,
                out var destinationError))
        {
            throw new InvalidOperationException(destinationError ?? "Destination spawner rectangle is invalid.");
        }
    }

    private static XDocument LoadDestinationDocument(string destinationPath)
    {
        if (File.Exists(destinationPath))
        {
            var document = XDocument.Load(destinationPath, LoadOptions.PreserveWhitespace);
            if (document.Root?.Name != "Spawns")
            {
                throw new InvalidDataException("Invalid destination spawns xml: root element must be <Spawns>.");
            }

            return document;
        }

        return new XDocument(new XElement("Spawns"));
    }

    private static void SaveDestinationDocument(string destinationPath, XDocument document)
    {
        var directory = Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(destinationPath))
        {
            File.Copy(destinationPath, destinationPath + ".bak", overwrite: true);
        }

        document.Save(destinationPath);
    }

    private static bool CentreInSourceRect(SpawnsCopyRequest request, int centreX, int centreY)
    {
        return centreX >= request.SourceX1
               && centreX <= request.SourceX2
               && centreY >= request.SourceY1
               && centreY <= request.SourceY2;
    }

    private static void ReportProgress(Action<int, string>? progress, int current, int total, int copied, string phase)
    {
        if (progress == null)
        {
            return;
        }

        if (current < total && current % 128 != 0)
        {
            return;
        }

        var percent = (int)(current / (double)total * 100d);
        progress(percent, $"{phase}... {current}/{total} (copied {copied})");
    }

    private static int ParseInt(string? value)
    {
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : 0;
    }

    private static void SetInt(XElement element, XName name, int value)
    {
        SetValue(element, name, value.ToString(CultureInfo.InvariantCulture));
    }

    private static void SetValue(XElement element, XName name, string value)
    {
        var child = element.Element(name);
        if (child == null)
        {
            element.Add(new XElement(name, value));
            return;
        }

        child.Value = value;
    }
}
