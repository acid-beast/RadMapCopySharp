using System.Runtime.InteropServices;

namespace RadMapCopySharp.Core
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MapCell
    {
        public ushort TileID;
        public sbyte Altitude;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MapBlock
    {
        public uint Header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public MapCell[] Cells;

        public static MapBlock Create()
        {
            return new MapBlock { Cells = new MapCell[64] };
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StaticIndex
    {
        public int Lookup;
        public int Length;
        public int Extra;
    }
}

