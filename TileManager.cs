using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameProgII_OOPMapSystem_Isaac_20250205
{
    internal class TileManager
    {
        private Dictionary<Tile.TileType, Tile> tileData;

        private Texture2D tileAtlas;

        //string basePath = AppDomain.CurrentDomain.BaseDirectory;
                
        public string filePath = "";

       
        public TileManager(ContentManager content)
        {
            tileData = new Dictionary<Tile.TileType, Tile>();

            tileAtlas = content.Load<Texture2D>("tilemap_packed");

            tileData[Tile.TileType.Grass] = new Tile(Tile.TileType.Grass, Tile.TileStatus.isWalkable, new Rectangle(0, 0, 16, 16));
            tileData[Tile.TileType.Dirt] = new Tile(Tile.TileType.Dirt, Tile.TileStatus.isWalkable, new Rectangle(16, 0, 16, 16));
            //tileData[Tile.TileType.Border] = new Tile(Tile.TileType.Border, Tile.TileStatus.isWalkable, new Rectangle(0, 0, 32, 32));

        }
        
        public char[,] LoadMapFromFile(string filePath)
        {           
            List<string> lines = new List<string>();
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;                

                while ((line = reader.ReadLine()) != null)
                {                                        
                    //removing delimiter
                    lines.Add(line.Replace(",", ""));
                }
            }
            
            int rows = lines.Count;
            int cols = lines[0].Length;

            //2d array to store values from the file (lines)
            char[,] tileArray = new char[rows, cols];

            //looping through lines and adding each char to the array in the correct position
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {                    
                    tileArray[i, j] = lines[i][j];                                                                                                                       
                }
            }                                    
            
            //returning a 2D array that will be used to generate(draw) the tilemap
            return tileArray;
        }

        void GenerateTileMap(char[,] mapData)
        {

        }

        void GenerateTileMap()
        {

        }

        //method to map the numbers inside a csv to Tiles in my dictionary using their keys (TileType)
        private Tile ConverCharToTile(char tileValue)
        {            
            Tile.TileType tileType = tileValue switch
            {
                '0' => Tile.TileType.Grass, // grass
                '1' => Tile.TileType.Dirt, // dirt
                _ => Tile.TileType.Border // default
            };
            return tileData[tileType];
        }               

        void Update(GameTime gameTime)
        {

        }

        //method to draw each tile from a given 2D character array in the correct size and position
        public void Draw(SpriteBatch spriteBatch)
        {
            char[,] tileMap = LoadMapFromFile(filePath);
            
            //to account for the size of the tile when drawing
            int tileWidth = 16;
            int tileHeight = 16; 

            //looping through each row and column
            for (int row = 0; row < tileMap.GetLength(0); row++)
            {
                for (int col = 0; col < tileMap.GetLength(1); col++)
                {                    
                    //converting the current value in the row/column of the array to a Tile 
                    char tileValue = tileMap[row, col];
                    Tile tile = ConverCharToTile(tileValue);
                                        
                    //determing the position to write the tiles at using their dimensions and the position in the array                 
                    Vector2 position = new Vector2(col * tileWidth, row * tileHeight);


                    //Debug.WriteLine(sourceRect);
                    spriteBatch.Draw(tileAtlas, position, tile.SourceRect, Color.White);
                }
            }
        }
    }
}
