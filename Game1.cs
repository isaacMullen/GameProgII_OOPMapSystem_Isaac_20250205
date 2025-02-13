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

        Texture2D playerTexture;      
        //player will remain null until the map is drawn and the List of walkable tiles is full
        Player player = null;
       

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
            tileManager = new(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {            
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //finding the relative path of the CSV file. searching based on project setup (assumes .csv lives inside the main .csproj folder)
            string currentDir = Directory.GetCurrentDirectory();
            string projectDir = Directory.GetParent(currentDir).Parent.Parent.FullName;

            //combine the path with the 'maps' folder
            string mapsFolderPath = Path.Combine(projectDir, "maps");

            if(Directory.Exists(mapsFolderPath))
            {
                string[] files = Directory.GetFiles(mapsFolderPath);
                
                foreach(var file in files)
                {
                    tileManager.maps.Add(file);
                }
            }
            //------------------------INITIALLY LOADING THE MAP (BOOL TOGGLE FOR GENERATION VS LOADING FROM FILES)------------------------
            tileManager.CacheMapData(false);
            
            //loading player
            playerTexture = Content.Load<Texture2D>("tile_0160");            
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //if the player is still null and the walkable tiles list has entries, we spawn the player
            if(player == null && tileManager.walkableTiles.Count > 0)
            {
                player = new("Player", playerTexture, tileManager);
            }
            
            player?.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            tileManager.Draw(_spriteBatch);
            player?.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }       
    }
}