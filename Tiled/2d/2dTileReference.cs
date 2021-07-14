using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace Tiled.TwoDimensions
{
    public sealed class TwoDimensionTileReference : ITile
    {
        public const int Type_Solid = 0;

        public const int Type_Halfbrick = 1;

        public const int Type_SlopeDownRight = 2;

        public const int Type_SlopeDownLeft = 3;

        public const int Type_SlopeUpRight = 4;

        public const int Type_SlopeUpLeft = 5;

        public const int Liquid_Water = 0;

        public const int Liquid_Lava = 1;

        public const int Liquid_Honey = 2;

        public int collisionType
        {
            get
            {
                if (!this.active())
                {
                    return 0;
                }
                if (this.halfBrick())
                {
                    return 2;
                }
                if (this.slope() > 0)
                {
                    return (2 + this.slope());
                }
                if (Main.tileSolid[this.type] && !Main.tileSolidTop[this.type])
                {
                    return 1;
                }
                return -1;
            }
        }

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

        public short sTileHeader
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
        private readonly StructTile[,] data;

        public TwoDimensionTileReference(StructTile[,] data, int x, int y) // not shorts because they will be converted to an int when accessing arrays
        {
            this.x = x;
            this.y = y;
            this.data = data;
		}

		public void CopyFrom(ITile from)
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


		public new string ToString()
		{
			return $"Tile Type:{type} Active:{active()} Wall:{wall} Slope:{slope()} fX:{frameX} fY:{frameY}";
		}


		public object Clone()
		{
			return MemberwiseClone();
		}

		public void ClearEverything()
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

		public void ClearTile()
		{
			slope(0);
			halfBrick(halfBrick: false);
			active(active: false);
			inActive(inActive: false);
		}

		public bool isTheSameAs(ITile compTile)
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
			return true;
		}

		public int blockType()
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

		public void liquidType(int liquidType)
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
			}
		}

		public byte liquidType()
		{
			return (byte)((bTileHeader & 0x60) >> 5);
		}

		public bool nactive()
		{
			if ((sTileHeader & 0x60) == 32)
			{
				return true;
			}
			return false;
		}

		public void ResetToType(ushort type)
		{
			liquid = 0;
			sTileHeader = 32;
			bTileHeader = 0;
			bTileHeader2 = 0;
			bTileHeader3 = 0;
			frameX = 0;
			frameY = 0;
			((ITile)this).type = type;
		}

		public void ClearMetadata()
		{
			liquid = 0;
			sTileHeader = 0;
			bTileHeader = 0;
			bTileHeader2 = 0;
			bTileHeader3 = 0;
			frameX = 0;
			frameY = 0;
		}

		public Color actColor(Color oldColor)
		{
			if (!inActive())
			{
				return oldColor;
			}
			double num = 0.4;
			return new Color((byte)(num * (double)(int)oldColor.R), (byte)(num * (double)(int)oldColor.G), (byte)(num * (double)(int)oldColor.B), oldColor.A);
		}

		public void actColor(ref Vector3 oldColor)
		{
			if (inActive())
			{
				oldColor *= 0.4f;
			}
		}

		public bool topSlope()
		{
			byte b = slope();
			if (b != 1)
			{
				return b == 2;
			}
			return true;
		}

		public bool bottomSlope()
		{
			byte b = slope();
			if (b != 3)
			{
				return b == 4;
			}
			return true;
		}

		public bool leftSlope()
		{
			byte b = slope();
			if (b != 2)
			{
				return b == 4;
			}
			return true;
		}

		public bool rightSlope()
		{
			byte b = slope();
			if (b != 1)
			{
				return b == 3;
			}
			return true;
		}

		public bool HasSameSlope(ITile tile)
		{
			return (sTileHeader & 0x7400) == (tile.sTileHeader & 0x7400);
		}

		public byte wallColor()
		{
			return (byte)(bTileHeader & 0x1F);
		}

		public void wallColor(byte wallColor)
		{
			bTileHeader = (byte)((bTileHeader & 0xE0) | wallColor);
		}

		public bool lava()
		{
			return (bTileHeader & 0x20) == 32;
		}

		public void lava(bool lava)
		{
			if (lava)
			{
				bTileHeader = (byte)((bTileHeader & 0x9F) | 0x20);
			}
			else
			{
				bTileHeader &= 223;
			}
		}

		public bool honey()
		{
			return (bTileHeader & 0x40) == 64;
		}

		public void honey(bool honey)
		{
			if (honey)
			{
				bTileHeader = (byte)((bTileHeader & 0x9F) | 0x40);
			}
			else
			{
				bTileHeader &= 191;
			}
		}

		public bool wire4()
		{
			return (bTileHeader & 0x80) == 128;
		}

		public void wire4(bool wire4)
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

		public int wallFrameX()
		{
			return (bTileHeader2 & 0xF) * 36;
		}

		public void wallFrameX(int wallFrameX)
		{
			bTileHeader2 = (byte)((bTileHeader2 & 0xF0) | ((wallFrameX / 36) & 0xF));
		}

		public byte frameNumber()
		{
			return (byte)((bTileHeader2 & 0x30) >> 4);
		}

		public void frameNumber(byte frameNumber)
		{
			bTileHeader2 = (byte)((bTileHeader2 & 0xCF) | ((frameNumber & 3) << 4));
		}

		public byte wallFrameNumber()
		{
			return (byte)((bTileHeader2 & 0xC0) >> 6);
		}

		public void wallFrameNumber(byte wallFrameNumber)
		{
			bTileHeader2 = (byte)((bTileHeader2 & 0x3F) | ((wallFrameNumber & 3) << 6));
		}

		public int wallFrameY()
		{
			return (bTileHeader3 & 7) * 36;
		}

		public void wallFrameY(int wallFrameY)
		{
			bTileHeader3 = (byte)((bTileHeader3 & 0xF8) | ((wallFrameY / 36) & 7));
		}

		public bool checkingLiquid()
		{
			return (bTileHeader3 & 8) == 8;
		}

		public void checkingLiquid(bool checkingLiquid)
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

		public bool skipLiquid()
		{
			return (bTileHeader3 & 0x10) == 16;
		}

		public void skipLiquid(bool skipLiquid)
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

		public byte color()
		{
			return (byte)(sTileHeader & 0x1F);
		}

		public void color(byte color)
		{
			sTileHeader = (short)((sTileHeader & 0xFFE0) | color);
		}

		public bool active()
		{
			return (sTileHeader & 0x20) == 32;
		}

		public void active(bool active)
		{
			if (active)
			{
				sTileHeader |= 32;
			}
			else
			{
				sTileHeader = (short)(sTileHeader & 0xFFDF);
			}
		}

		public bool inActive()
		{
			return (sTileHeader & 0x40) == 64;
		}

		public void inActive(bool inActive)
		{
			if (inActive)
			{
				sTileHeader |= 64;
			}
			else
			{
				sTileHeader = (short)(sTileHeader & 0xFFBF);
			}
		}

		public bool wire()
		{
			return (sTileHeader & 0x80) == 128;
		}

		public void wire(bool wire)
		{
			if (wire)
			{
				sTileHeader |= 128;
			}
			else
			{
				sTileHeader = (short)(sTileHeader & 0xFF7F);
			}
		}

		public bool wire2()
		{
			return (sTileHeader & 0x100) == 256;
		}

		public void wire2(bool wire2)
		{
			if (wire2)
			{
				sTileHeader |= 256;
			}
			else
			{
				sTileHeader = (short)(sTileHeader & 0xFEFF);
			}
		}

		public bool wire3()
		{
			return (sTileHeader & 0x200) == 512;
		}

		public void wire3(bool wire3)
		{
			if (wire3)
			{
				sTileHeader |= 512;
			}
			else
			{
				sTileHeader = (short)(sTileHeader & 0xFDFF);
			}
		}

		public bool halfBrick()
		{
			return (sTileHeader & 0x400) == 1024;
		}

		public void halfBrick(bool halfBrick)
		{
			if (halfBrick)
			{
				sTileHeader |= 1024;
			}
			else
			{
				sTileHeader = (short)(sTileHeader & 0xFBFF);
			}
		}

		public bool actuator()
		{
			return (sTileHeader & 0x800) == 2048;
		}

		public void actuator(bool actuator)
		{
			if (actuator)
			{
				sTileHeader |= 2048;
			}
			else
			{
				sTileHeader = (short)(sTileHeader & 0xF7FF);
			}
		}

		public byte slope()
		{
			return (byte)((sTileHeader & 0x7000) >> 12);
		}

		public void slope(byte slope)
		{
			sTileHeader = (short)((sTileHeader & 0x8FFF) | ((slope & 7) << 12));
		}

		public void Clear(TileDataType types)
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
				color(0);
			}
			if ((types & TileDataType.WallPaint) != 0)
			{
				wallColor(0);
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

		public void Initialise()
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
	}
}
