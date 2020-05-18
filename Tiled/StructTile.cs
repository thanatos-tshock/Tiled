using System.Runtime.InteropServices;

namespace Tiled
{
    /// <summary>
    /// StructTile maps the tile data directly to memory rather than having to manipulate bytes
    /// </summary>
	[StructLayout(LayoutKind.Sequential, Size = 13, Pack = 1)]
	public struct StructTile
	{
		public ushort wall;
		public byte liquid;
		public byte bTileHeader;
		public byte bTileHeader2;
		public byte bTileHeader3;
		public ushort type;
		public short sTileHeader;
		public short frameX;
		public short frameY;
	}
}
