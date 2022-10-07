using System;
using System.Runtime.CompilerServices;
using Terraria;

namespace Tiled.TwoDimensions;

public class TwoDimensionTileProvider : ModFramework.ICollection<ITile>, IDisposable
{
	private StructTile[,] data;
	private int _width;
	private int _height;

	public int Width => this._width;
	public int Height => this._height;

    public ITile this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
		{
			if (data == null)
			{
				data = new StructTile[Main.maxTilesX + 1, Main.maxTilesY + 1];

				this._width = Main.maxTilesX + 1;
				this._height = Main.maxTilesY + 1;
			}

			return new TwoDimensionTileReference(data, x, y);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
		{
			ITile tile = new TwoDimensionTileReference(data, x, y);
			tile.CopyFrom(value);
		}
	}

	public void Dispose()
	{
		if (data != null)
		{
			for (var x = 0; x < data.GetLength(0); x++)
			{
				for (var y = 0; y < data.GetLength(1); y++)
				{
					data[x, y].bTileHeader = 0;
					data[x, y].bTileHeader2 = 0;
					data[x, y].bTileHeader3 = 0;
					data[x, y].frameX = 0;
					data[x, y].frameY = 0;
					data[x, y].liquid = 0;
					data[x, y].type = 0;
					data[x, y].wall = 0;
				}
			}
			data = null;
		}
	}
}
