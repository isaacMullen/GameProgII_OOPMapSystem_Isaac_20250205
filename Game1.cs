using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace GameProgII_OOPMapSystem_Isaac_20250205
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        TileManager tileManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;      
            
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 360;                        
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            tileManager = new(Content);
            
            //finding the relative path of the CSV file. searching based on project setup (assumes .csv lives inside the main .csproj folder)
            string currentDir = Directory.GetCurrentDirectory();
            
            string projectDir = Directory.GetParent(currentDir).Parent.Parent.FullName;
                       
            tileManager.filePath = Path.Combine(projectDir, "map.csv");

            //test
            Console.WriteLine("File path: " + tileManager.filePath);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            tileManager.LoadMapFromFile(tileManager.filePath);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            tileManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
