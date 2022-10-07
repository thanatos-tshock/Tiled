using System;
using System.Runtime.CompilerServices;
using Terraria;

namespace Tiled.OneDimension;

public class OneDimensionTileProvider : ModFramework.ICollection<ITile>, IDisposable
{
    private StructTile[] data;
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
                data = new StructTile[(Main.maxTilesX + 1) * (Main.maxTilesY + 1)];

                this._width = Main.maxTilesX + 1;
                this._height = Main.maxTilesY + 1;
            }

            return new OneDimensionTileReference(data, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            ITile tile = new OneDimensionTileReference(data, x, y);
            tile.CopyFrom(value);
        }
    }

    public void Dispose()
    {
        if (data != null)
        {
            for (var x = 0; x < data.Length; x++)
            {
                data[x].bTileHeader = 0;
                data[x].bTileHeader2 = 0;
                data[x].bTileHeader3 = 0;
                data[x].frameX = 0;
                data[x].frameY = 0;
                data[x].liquid = 0;
                data[x].type = 0;
                data[x].wall = 0;
            }
            data = null;
        }
    }
}
