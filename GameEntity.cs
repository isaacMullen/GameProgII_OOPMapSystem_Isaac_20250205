using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace GameProgII_OOPMapSystem_Isaac_20250205
{
    internal class GameEntity
    {
        public Texture2D Texture { get; private set; }
        public string Name { get; private set; }               
        public Vector2 Position { get; protected set; } = Vector2.Zero;
        public Vector2 Direction { get; protected set; } = Vector2.Zero;
        public float GridSize { get; protected set; } = 16;

        public GameEntity(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;            
        }

        public virtual void Move()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void UpdateDirection()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}
