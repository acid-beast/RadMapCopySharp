using System;
using System.IO;
using System.Collections.Generic;
using RadMapCopySharp.Core;

namespace RadMapCopySharp.Core
{
    public static class MapOperations
    {
        public delegate void ProgressCallback(int percent, string message);

        public static void CopyRegion(
            string srcPath, string dstPath,
            int startX, int startY, int endX, int endY,
            int destX, int destY,
            int mapHeightBlocks,
            int zOffset, bool useRandomZ, int zRandMin, int zRandMax,
            HashSet<ushort>? skipTiles,
            ProgressCallback? onProgress)
        {
            using var srcFs = new FileStream(srcPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var dstFs = new FileStream(dstPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            int width = endX - startX + 1;
            int height = endY - startY + 1;
            int total = width * height;
            int current = 0;

            Random rand = new Random();

            for (int x = startX; x <= endX; x++)
            {
                int dx = destX + (x - startX);
                for (int y = startY; y <= endY; y++)
                {
                    int dy = destY + (y - startY);

                    // Source offsets
                    int sBlockX = x / 8;
                    int sBlockY = y / 8;
                    int sOffX = x % 8;
                    int sOffY = y % 8;

                    // Dest offsets
                    int dBlockX = dx / 8;
                    int dBlockY = dy / 8;
                    int dOffX = dx % 8;
                    int dOffY = dy % 8;

                    // Calculate file positions
                    // 196 = SizeOf(MapBlock)
                    long srcPos = (long)(sBlockX * mapHeightBlocks + sBlockY) * 196 + 4 + (sOffY * 8 + sOffX) * 3;
                    long dstPos = (long)(dBlockX * mapHeightBlocks + dBlockY) * 196 + 4 + (dOffY * 8 + dOffX) * 3;

                    srcFs.Seek(srcPos, SeekOrigin.Begin);
                    ushort tileId = (ushort)(srcFs.ReadByte() | (srcFs.ReadByte() << 8));
                    sbyte altitude = (sbyte)srcFs.ReadByte();

                    if (skipTiles != null && skipTiles.Contains(tileId))
                    {
                        continue;
                    }

                    int newZ = altitude;
                    if (useRandomZ)
                    {
                        newZ = rand.Next(zRandMin, zRandMax + 1);
                    }
                    else
                    {
                        newZ += zOffset;
                    }
                    
                    // Clamp Z to sbyte
                    if (newZ > 127) newZ = 127;
                    if (newZ < -128) newZ = -128;

                    dstFs.Seek(dstPos, SeekOrigin.Begin);
                    dstFs.WriteByte((byte)(tileId & 0xFF));
                    dstFs.WriteByte((byte)((tileId >> 8) & 0xFF));
                    dstFs.WriteByte((byte)(sbyte)newZ);

                    current++;
                    if (current % 1000 == 0)
                    {
                        onProgress?.Invoke((int)((float)current / total * 100), $"Copying tile {x},{y}...");
                    }
                }
            }
            onProgress?.Invoke(100, "Copy complete.");
        }

        public static void CreateNewMap(string path, int width, int height, ushort defaultTile, sbyte defaultZ, ProgressCallback? onProgress)
        {
            int blocksX = width / 8;
            int blocksY = height / 8;
            int totalBlocks = blocksX * blocksY;

            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = new BinaryWriter(fs);

            for (int i = 0; i < totalBlocks; i++)
            {
                writer.Write((uint)0); // Header
                for (int cell = 0; cell < 64; cell++)
                {
                    writer.Write(defaultTile);
                    writer.Write(defaultZ);
                }

                if (i % 1000 == 0)
                {
                    onProgress?.Invoke((int)((float)i / totalBlocks * 100), $"Creating block {i} of {totalBlocks}...");
                }
            }
            onProgress?.Invoke(100, "Map created successfully.");
        }
        
        public static void ExtendToML(string srcPath, string dstPath, int oldWidth, int oldHeight, ProgressCallback? onProgress)
        {
            // ML is always 7168x4096 (896x512 blocks)
            int mlBlocksY = 512;
            int oldBlocksY = oldHeight / 8;

            // Step 1: Create blank ML map
            CreateNewMap(dstPath, 7168, 4096, 0, 0, (p, m) => onProgress?.Invoke(p / 2, "Initializing ML layout..."));

            // Step 2: Copy old content into it
            CopyRegion(srcPath, dstPath, 0, 0, oldWidth - 1, oldHeight - 1, 0, 0, mlBlocksY, 0, false, 0, 0, null, (p, m) => onProgress?.Invoke(50 + (p / 2), "Migrating data..."));
            
            onProgress?.Invoke(100, "Map extension complete.");
        }
    }
}

