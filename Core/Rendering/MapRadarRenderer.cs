using System.Drawing;
using System.Drawing.Imaging;
using RadMapCopySharp.Core.IO;

namespace RadMapCopySharp.Core.Rendering;

public sealed class MapRadarRenderer
{
    public Bitmap RenderFullMap(ILandMapFile mapFile, RadarColorTable colorTable, IProgress<int>? progress = null)
    {
        var width = mapFile.Profile.Width;
        var height = mapFile.Profile.Height;

        var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        var rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var data = bitmap.LockBits(rectangle, ImageLockMode.WriteOnly, bitmap.PixelFormat);
        var totalBlocks = mapFile.Profile.WidthBlocks * mapFile.Profile.HeightBlocks;
        var completedBlocks = 0;

        try
        {
            unsafe
            {
                var basePtr = (byte*)data.Scan0;
                Span<ushort> blockTiles = stackalloc ushort[64];

                for (var blockX = 0; blockX < mapFile.Profile.WidthBlocks; blockX++)
                {
                    var baseX = blockX * 8;
                    for (var blockY = 0; blockY < mapFile.Profile.HeightBlocks; blockY++)
                    {
                        var baseY = blockY * 8;
                        mapFile.ReadLandBlockTiles(blockX, blockY, blockTiles);

                        for (var cellY = 0; cellY < 8; cellY++)
                        {
                            var row = basePtr + ((baseY + cellY) * data.Stride);
                            var tileRow = cellY * 8;
                            for (var cellX = 0; cellX < 8; cellX++)
                            {
                                var mapX = baseX + cellX;
                                var tileId = blockTiles[tileRow + cellX];
                                var color = colorTable.GetLandColor(tileId);

                                var p = row + (mapX * 4);
                                p[0] = color.B;
                                p[1] = color.G;
                                p[2] = color.R;
                                p[3] = 255;
                            }
                        }

                        completedBlocks++;
                        if (completedBlocks % 256 == 0 || completedBlocks == totalBlocks)
                        {
                            var percent = (int)(completedBlocks * 100.0 / totalBlocks);
                            progress?.Report(percent);
                        }
                    }
                }
            }
        }
        finally
        {
            bitmap.UnlockBits(data);
        }

        return bitmap;
    }
}
