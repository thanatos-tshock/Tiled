using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace Tiled.Struct;

//prone to copies on the stack if youre not careful
public struct StructuredTileReference : TiledTile
{
    public ushort type
    {
        get { return data[offset].type; }
        set { data[offset].type = value; }
    }

    public ushort wall
    {
        get { return data[offset].wall; }
        set { data[offset].wall = value; }
    }

    public byte liquid
    {
        get { return data[offset].liquid; }
        set { data[offset].liquid = value; }
    }

    public ushort sTileHeader
    {
        get { return data[offset].sTileHeader; }
        set { data[offset].sTileHeader = value; }
    }

    public byte bTileHeader
    {
        get { return data[offset].bTileHeader; }
        set { data[offset].bTileHeader = value; }
    }

    public byte bTileHeader2
    {
        get { return data[offset].bTileHeader2; }
        set { data[offset].bTileHeader2 = value; }
    }

    public byte bTileHeader3
    {
        get { return data[offset].bTileHeader3; }
        set { data[offset].bTileHeader3 = value; }
    }

    public short frameX
    {
        get { return data[offset].frameX; }
        set { data[offset].frameX = value; }
    }

    public short frameY
    {
        get { return data[offset].frameY; }
        set { data[offset].frameY = value; }
    }

    internal readonly int offset;
    private StructTile[] data;

    public StructuredTileReference(StructTile[] data, int x, int y) // not shorts because I rather save a number of convert opcodes
    {
        this.offset = Terraria.Main.maxTilesY * x + y;
        this.data = data;
    }

    public object Clone() => MemberwiseClone();
}
