using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace Tiled.TwoDimensions;

public sealed class TwoDimensionTileReference : TiledTile
{
    public ushort type
    {
        get { return data[x, y].type; }
        set { data[x, y].type = value; }
    }

    public ushort wall
    {
        get { return data[x, y].wall; }
        set { data[x, y].wall = value; }
    }

    public byte liquid
    {
        get { return data[x, y].liquid; }
        set { data[x, y].liquid = value; }
    }

    public ushort sTileHeader
    {
        get { return data[x, y].sTileHeader; }
        set { data[x, y].sTileHeader = value; }
    }

    public byte bTileHeader
    {
        get { return data[x, y].bTileHeader; }
        set { data[x, y].bTileHeader = value; }
    }

    public byte bTileHeader2
    {
        get { return data[x, y].bTileHeader2; }
        set { data[x, y].bTileHeader2 = value; }
    }

    public byte bTileHeader3
    {
        get { return data[x, y].bTileHeader3; }
        set { data[x, y].bTileHeader3 = value; }
    }

    public short frameX
    {
        get { return data[x, y].frameX; }
        set { data[x, y].frameX = value; }
    }

    public short frameY
    {
        get { return data[x, y].frameY; }
        set { data[x, y].frameY = value; }
    }

    internal readonly int x, y;
    private StructTile[,] data;

    public TwoDimensionTileReference(StructTile[,] data, int x, int y) // not shorts because they will be converted to an int when accessing arrays
    {
        this.x = x;
        this.y = y;
        this.data = data;
    }

    public object Clone() => MemberwiseClone();
}
