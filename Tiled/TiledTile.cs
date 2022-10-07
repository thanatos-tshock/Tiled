using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace Tiled;

public interface TiledTile : ITile
{
    int ITile.collisionType
    {
        get
        {
            if (!active())
            {
                return 0;
            }
            if (halfBrick())
            {
                return 2;
            }
            if (slope() > 0)
            {
                return 2 + slope();
            }
            if (Main.tileSolid[type] && !Main.tileSolidTop[type])
            {
                return 1;
            }
            return -1;
        }
    }

    void ITile.ClearEverything()
    {
        type = 0;
        wall = 0;
        liquid = 0;
        sTileHeader = 0;
        bTileHeader = 0;
        bTileHeader2 = 0;
        bTileHeader3 = 0;
        frameX = 0;
        frameY = 0;
    }

    void ITile.ClearTile()
    {
        slope(0);
        halfBrick(halfBrick: false);
        active(active: false);
        inActive(inActive: false);
    }

    void ITile.CopyFrom(ITile from)
    {
        type = from.type;
        wall = from.wall;
        liquid = from.liquid;
        sTileHeader = from.sTileHeader;
        bTileHeader = from.bTileHeader;
        bTileHeader2 = from.bTileHeader2;
        bTileHeader3 = from.bTileHeader3;
        frameX = from.frameX;
        frameY = from.frameY;
    }

    bool ITile.isTheSameAs(ITile compTile)
    {
        if (compTile == null)
        {
            return false;
        }
        if (sTileHeader != compTile.sTileHeader)
        {
            return false;
        }
        if (active())
        {
            if (type != compTile.type)
            {
                return false;
            }
            if (Main.tileFrameImportant[type] && (frameX != compTile.frameX || frameY != compTile.frameY))
            {
                return false;
            }
        }
        if (wall != compTile.wall || liquid != compTile.liquid)
        {
            return false;
        }
        if (compTile.liquid == 0)
        {
            if (wallColor() != compTile.wallColor())
            {
                return false;
            }
            if (wire4() != compTile.wire4())
            {
                return false;
            }
        }
        else if (bTileHeader != compTile.bTileHeader)
        {
            return false;
        }
        if (invisibleBlock() != compTile.invisibleBlock() || invisibleWall() != compTile.invisibleWall() || fullbrightBlock() != compTile.fullbrightBlock() || fullbrightWall() != compTile.fullbrightWall())
        {
            return false;
        }
        return true;
    }

    int ITile.blockType()
    {
        if (halfBrick())
        {
            return 1;
        }
        int num = slope();
        if (num > 0)
        {
            num++;
        }
        return num;
    }

    void ITile.liquidType(int liquidType)
    {
        switch (liquidType)
        {
            case 0:
                bTileHeader &= 159;
                break;
            case 1:
                lava(lava: true);
                break;
            case 2:
                honey(honey: true);
                break;
            case 3:
                shimmer(shimmer: true);
                break;
        }
    }

    byte ITile.liquidType()
    {
        return (byte)((bTileHeader & 0x60) >> 5);
    }

    bool ITile.nactive()
    {
        if ((sTileHeader & 0x60) == 32)
        {
            return true;
        }
        return false;
    }

    void ITile.ResetToType(ushort type)
    {
        liquid = 0;
        sTileHeader = 32;
        bTileHeader = 0;
        bTileHeader2 = 0;
        bTileHeader3 = 0;
        frameX = 0;
        frameY = 0;
        this.type = type;
    }

    void ITile.ClearMetadata()
    {
        liquid = 0;
        sTileHeader = 0;
        bTileHeader = 0;
        bTileHeader2 = 0;
        bTileHeader3 = 0;
        frameX = 0;
        frameY = 0;
    }

    Color ITile.actColor(Color oldColor)
    {
        if (!inActive())
        {
            return oldColor;
        }
        double num = 0.4;
        return new Color((byte)(num * (double)(int)oldColor.R), (byte)(num * (double)(int)oldColor.G), (byte)(num * (double)(int)oldColor.B), oldColor.A);
    }

    void ITile.actColor(ref Vector3 oldColor)
    {
        if (inActive())
        {
            oldColor *= 0.4f;
        }
    }

    bool ITile.topSlope()
    {
        byte b = slope();
        if (b != 1)
        {
            return b == 2;
        }
        return true;
    }

    bool ITile.bottomSlope()
    {
        byte b = slope();
        if (b != 3)
        {
            return b == 4;
        }
        return true;
    }

    bool ITile.leftSlope()
    {
        byte b = slope();
        if (b != 2)
        {
            return b == 4;
        }
        return true;
    }

    bool ITile.rightSlope()
    {
        byte b = slope();
        if (b != 1)
        {
            return b == 3;
        }
        return true;
    }

    bool ITile.HasSameSlope(ITile tile)
    {
        return (sTileHeader & 0x7400) == (tile.sTileHeader & 0x7400);
    }

    byte ITile.wallColor()
    {
        return (byte)(bTileHeader & 0x1Fu);
    }

    void ITile.wallColor(byte wallColor)
    {
        bTileHeader = (byte)((bTileHeader & 0xE0u) | wallColor);
    }

    bool ITile.lava()
    {
        return (bTileHeader & 0x60) == 32;
    }

    void ITile.lava(bool lava)
    {
        if (lava)
        {
            bTileHeader = (byte)((bTileHeader & 0x9Fu) | 0x20u);
        }
        else
        {
            bTileHeader &= 223;
        }
    }

    bool ITile.honey()
    {
        return (bTileHeader & 0x60) == 64;
    }

    void ITile.honey(bool honey)
    {
        if (honey)
        {
            bTileHeader = (byte)((bTileHeader & 0x9Fu) | 0x40u);
        }
        else
        {
            bTileHeader &= 191;
        }
    }

    bool ITile.shimmer()
    {
        return (bTileHeader & 0x60) == 96;
    }

    void ITile.shimmer(bool shimmer)
    {
        if (shimmer)
        {
            bTileHeader = (byte)((bTileHeader & 0x9Fu) | 0x60u);
        }
        else
        {
            bTileHeader &= 159;
        }
    }

    bool ITile.wire4()
    {
        return (bTileHeader & 0x80) == 128;
    }

    void ITile.wire4(bool wire4)
    {
        if (wire4)
        {
            bTileHeader |= 128;
        }
        else
        {
            bTileHeader &= 127;
        }
    }

    int ITile.wallFrameX()
    {
        return (bTileHeader2 & 0xF) * 36;
    }

    void ITile.wallFrameX(int wallFrameX)
    {
        bTileHeader2 = (byte)((bTileHeader2 & 0xF0u) | ((uint)(wallFrameX / 36) & 0xFu));
    }

    byte ITile.frameNumber()
    {
        return (byte)((bTileHeader2 & 0x30) >> 4);
    }

    void ITile.frameNumber(byte frameNumber)
    {
        bTileHeader2 = (byte)((bTileHeader2 & 0xCFu) | (uint)((frameNumber & 3) << 4));
    }

    byte ITile.wallFrameNumber()
    {
        return (byte)((bTileHeader2 & 0xC0) >> 6);
    }

    void ITile.wallFrameNumber(byte wallFrameNumber)
    {
        bTileHeader2 = (byte)((bTileHeader2 & 0x3Fu) | (uint)((wallFrameNumber & 3) << 6));
    }

    int ITile.wallFrameY()
    {
        return (bTileHeader3 & 7) * 36;
    }

    void ITile.wallFrameY(int wallFrameY)
    {
        bTileHeader3 = (byte)((bTileHeader3 & 0xF8u) | ((uint)(wallFrameY / 36) & 7u));
    }

    bool ITile.checkingLiquid()
    {
        return (bTileHeader3 & 8) == 8;
    }

    void ITile.checkingLiquid(bool checkingLiquid)
    {
        if (checkingLiquid)
        {
            bTileHeader3 |= 8;
        }
        else
        {
            bTileHeader3 &= 247;
        }
    }

    bool ITile.skipLiquid()
    {
        return (bTileHeader3 & 0x10) == 16;
    }

    void ITile.skipLiquid(bool skipLiquid)
    {
        if (skipLiquid)
        {
            bTileHeader3 |= 16;
        }
        else
        {
            bTileHeader3 &= 239;
        }
    }

    bool ITile.invisibleBlock()
    {
        return (bTileHeader3 & 0x20) == 32;
    }

    void ITile.invisibleBlock(bool invisibleBlock)
    {
        if (invisibleBlock)
        {
            bTileHeader3 |= 32;
        }
        else
        {
            bTileHeader3 = (byte)(bTileHeader3 & 0xFFFFFFDFu);
        }
    }

    bool ITile.invisibleWall()
    {
        return (bTileHeader3 & 0x40) == 64;
    }

    void ITile.invisibleWall(bool invisibleWall)
    {
        if (invisibleWall)
        {
            bTileHeader3 |= 64;
        }
        else
        {
            bTileHeader3 = (byte)(bTileHeader3 & 0xFFFFFFBFu);
        }
    }

    bool ITile.fullbrightBlock()
    {
        return (bTileHeader3 & 0x80) == 128;
    }

    void ITile.fullbrightBlock(bool fullbrightBlock)
    {
        if (fullbrightBlock)
        {
            bTileHeader3 |= 128;
        }
        else
        {
            bTileHeader3 = (byte)(bTileHeader3 & 0xFFFFFF7Fu);
        }
    }

    byte ITile.color()
    {
        return (byte)(sTileHeader & 0x1Fu);
    }

    void ITile.color(byte color)
    {
        sTileHeader = (ushort)((sTileHeader & 0xFFE0u) | color);
    }

    bool ITile.active()
    {
        return (sTileHeader & 0x20) == 32;
    }

    void ITile.active(bool active)
    {
        if (active)
        {
            sTileHeader |= 32;
        }
        else
        {
            sTileHeader &= 65503;
        }
    }

    bool ITile.inActive()
    {
        return (sTileHeader & 0x40) == 64;
    }

    void ITile.inActive(bool inActive)
    {
        if (inActive)
        {
            sTileHeader |= 64;
        }
        else
        {
            sTileHeader &= 65471;
        }
    }

    bool ITile.wire()
    {
        return (sTileHeader & 0x80) == 128;
    }

    void ITile.wire(bool wire)
    {
        if (wire)
        {
            sTileHeader |= 128;
        }
        else
        {
            sTileHeader &= 65407;
        }
    }

    bool ITile.wire2()
    {
        return (sTileHeader & 0x100) == 256;
    }

    void ITile.wire2(bool wire2)
    {
        if (wire2)
        {
            sTileHeader |= 256;
        }
        else
        {
            sTileHeader &= 65279;
        }
    }

    bool ITile.wire3()
    {
        return (sTileHeader & 0x200) == 512;
    }

    void ITile.wire3(bool wire3)
    {
        if (wire3)
        {
            sTileHeader |= 512;
        }
        else
        {
            sTileHeader &= 65023;
        }
    }

    bool ITile.halfBrick()
    {
        return (sTileHeader & 0x400) == 1024;
    }

    void ITile.halfBrick(bool halfBrick)
    {
        if (halfBrick)
        {
            sTileHeader |= 1024;
        }
        else
        {
            sTileHeader &= 64511;
        }
    }

    bool ITile.actuator()
    {
        return (sTileHeader & 0x800) == 2048;
    }

    void ITile.actuator(bool actuator)
    {
        if (actuator)
        {
            sTileHeader |= 2048;
        }
        else
        {
            sTileHeader &= 63487;
        }
    }

    byte ITile.slope()
    {
        return (byte)((sTileHeader & 0x7000) >> 12);
    }

    void ITile.slope(byte slope)
    {
        sTileHeader = (ushort)((sTileHeader & 0x8FFFu) | (uint)((slope & 7) << 12));
    }

    bool ITile.fullbrightWall()
    {
        return (sTileHeader & 0x8000) == 32768;
    }

    void ITile.fullbrightWall(bool fullbrightWall)
    {
        if (fullbrightWall)
        {
            sTileHeader |= 32768;
        }
        else
        {
            sTileHeader = (ushort)(sTileHeader & 0xFFFF7FFFu);
        }
    }

    void ITile.Clear(TileDataType types)
    {
        if ((types & TileDataType.Tile) != 0)
        {
            type = 0;
            active(active: false);
            frameX = 0;
            frameY = 0;
        }
        if ((types & TileDataType.Wall) != 0)
        {
            wall = 0;
            wallFrameX(0);
            wallFrameY(0);
        }
        if ((types & TileDataType.TilePaint) != 0)
        {
            ClearBlockPaintAndCoating();
        }
        if ((types & TileDataType.WallPaint) != 0)
        {
            ClearWallPaintAndCoating();
        }
        if ((types & TileDataType.Liquid) != 0)
        {
            liquid = 0;
            liquidType(0);
            checkingLiquid(checkingLiquid: false);
        }
        if ((types & TileDataType.Slope) != 0)
        {
            slope(0);
            halfBrick(halfBrick: false);
        }
        if ((types & TileDataType.Wiring) != 0)
        {
            wire(wire: false);
            wire2(wire2: false);
            wire3(wire3: false);
            wire4(wire4: false);
        }
        if ((types & TileDataType.Actuator) != 0)
        {
            actuator(actuator: false);
            inActive(inActive: false);
        }
    }

    void ITile.CopyPaintAndCoating(ITile other)
    {
        color(other.color());
        invisibleBlock(other.invisibleBlock());
        fullbrightBlock(other.fullbrightBlock());
    }

    TileColorCache ITile.BlockColorAndCoating()
    {
        TileColorCache result = default(TileColorCache);
        result.Color = color();
        result.FullBright = fullbrightBlock();
        result.Invisible = invisibleBlock();
        return result;
    }

    TileColorCache ITile.WallColorAndCoating()
    {
        TileColorCache result = default(TileColorCache);
        result.Color = wallColor();
        result.FullBright = fullbrightWall();
        result.Invisible = invisibleWall();
        return result;
    }

    void ITile.UseBlockColors(TileColorCache cache)
    {
        cache.ApplyToBlock(this);
    }

    void ITile.UseWallColors(TileColorCache cache)
    {
        cache.ApplyToWall(this);
    }

    void ITile.ClearBlockPaintAndCoating()
    {
        color(0);
        fullbrightBlock(fullbrightBlock: false);
        invisibleBlock(invisibleBlock: false);
    }

    void ITile.ClearWallPaintAndCoating()
    {
        wallColor(0);
        fullbrightWall(fullbrightWall: false);
        invisibleWall(invisibleWall: false);
    }

    public virtual string ToString()
    {
        return "Tile Type:" + type + " Active:" + active().ToString() + " Wall:" + wall + " Slope:" + slope() + " fX:" + frameX + " fY:" + frameY;
    }
}
