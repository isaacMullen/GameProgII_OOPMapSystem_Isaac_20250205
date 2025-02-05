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

        public string filePath = "C:\\Isaacs Programming Projects\\GameProgII_OOPMapSystem_Isaac_20250205\\map.csv";

       
        public TileManager(ContentManager content)
        {
            tileData = new Dictionary<Tile.TileType, Tile>();

            tileAtlas = content.Load<Texture2D>("tilemap_packed");

            tileData[Tile.TileType.Grass] = new Tile(Tile.TileType.Grass, Tile.TileStatus.isWalkable, new Rectangle(0, 0, 32, 32));
            tileData[Tile.TileType.Dirt] = new Tile(Tile.TileType.Dirt, Tile.TileStatus.isWalkable, new Rectangle(16, 0, 32, 32));
            //tileData[Tile.TileType.Border] = new Tile(Tile.TileType.Border, Tile.TileStatus.isWalkable, new Rectangle(0, 0, 32, 32));

        }
        
        public char[,] LoadMapFromFile(string filePath)
        {
            //Debug.WriteLine("Started");

            List<string> lines = new List<string>();
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;                

                while ((line = reader.ReadLine()) != null)
                {                                        
                    lines.Add(line.Replace(",", ""));
                }
            }
            //Debug.WriteLine("Inside");
            int rows = lines.Count;
            int cols = lines[0].Length;

            //2d array to store values from the file (lines)
            char[,] tileArray = new char[rows, cols];

            //looping through lines and adding each char to the array in the correct position
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (lines[i][j] == ',')
                    {
                        Debug.WriteLine(lines[i][j]);
                    }
                    else
                    {
                        tileArray[i, j] = lines[i][j];
                    }                                                                                                   
                }
            }                        

            //looping through the array and printing the values at each index
            for (int i = 0; i < rows; i++)
            {
                string rowContent = "";

                for (int j = 0; j < cols; j++)
                {
                    rowContent += tileArray[i, j].ToString();
                }
                //Debug.WriteLine(rowContent);
            }
            return tileArray;
        }

        void GenerateTileMap(char[,] mapData)
        {

        }

        private Tile ConverCharToTile(char tileValue)
        {
            Tile.TileType tileType = tileValue switch
            {
                '0' => Tile.TileType.Grass, // grass
                '1' => Tile.TileType.Dirt,
                _ => Tile.TileType.Border // default
            };
            return tileData[tileType];
        }

        void GenerateTileMap()
        {

        }        

        void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            char[,] tileMap = LoadMapFromFile(filePath);
            
            int tileWidth = 32; // Width of each tile (assuming square tiles)
            int tileHeight = 32; // Height of each tile

            for (int row = 0; row < tileMap.GetLength(0); row++)
            {
                for (int col = 0; col < tileMap.GetLength(1); col++)
                {                    
                    
                    char tileValue = tileMap[row, col];
                    Tile tile = ConverCharToTile(tileValue);
                                        
                    
                    Rectangle sourceRect = tile.SourceRect; 
                    Vector2 position = new Vector2(col * tileWidth, row * tileHeight);


                    Debug.WriteLine(sourceRect);
                    spriteBatch.Draw(tileAtlas, position, sourceRect, Color.White);
                }
            }
        }
    }
}
