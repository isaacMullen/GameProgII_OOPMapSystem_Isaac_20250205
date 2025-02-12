using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProgII_OOPMapSystem_Isaac_20250205
{
    internal class Tile
    {      
        public enum TileType
        {
            Grass,
            Dirt,
            Border,
            Exit,
        }

        public enum TileStatus
        {
            isWalkable,
            NotWalkable,
        }

        public TileType Type { get; private set; }
        public TileStatus Status { get; set; }
        public Rectangle SourceRect { get; set; }


        public Tile(TileType tileType, TileStatus status, Rectangle sourceRect)
        {
            Type = tileType;
            Status = status;
            SourceRect = sourceRect;
        }
    }
}
