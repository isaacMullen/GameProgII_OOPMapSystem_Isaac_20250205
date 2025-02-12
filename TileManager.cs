using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace GameProgII_OOPMapSystem_Isaac_20250205
{
    internal class TileManager
    {
        public enum MapType
        {
            Loaded,
            Generated,
        }

        const int tileWidth = 16;
        const int tileHeight = 16;

        private Dictionary<Tile.TileType, Tile> tileData;

        private Texture2D tileAtlas;

        public List<Vector2> nonWalkableTiles = new();
        public List<Vector2> walkableTiles = new();
        public List<Vector2> exitTiles = new();

        //string basePath = AppDomain.CurrentDomain.BaseDirectory;

        public List<string> maps = new();

        public string filePath = "";

        public int MapIndex { get; set; } = 0;

        public char[,] currentMap;

        public MapType Type { get; private set; }

        public TileManager(ContentManager content)
        {
            tileData = new Dictionary<Tile.TileType, Tile>();

            tileAtlas = content.Load<Texture2D>("tilemap_packed");

            tileData[Tile.TileType.Grass] = new Tile(Tile.TileType.Grass, Tile.TileStatus.isWalkable, new Rectangle(0, 0, 16, 16));
            tileData[Tile.TileType.Dirt] = new Tile(Tile.TileType.Dirt, Tile.TileStatus.NotWalkable, new Rectangle(16, 0, 16, 16));
            tileData[Tile.TileType.Exit] = new Tile(Tile.TileType.Exit, Tile.TileStatus.isWalkable, new Rectangle(32, 0, 16, 16));
        }
        
        public char[,] LoadMapFromFile(string filePath)
        {
            Type = MapType.Loaded;

            walkableTiles.Clear();
            nonWalkableTiles.Clear();
            exitTiles.Clear();
            
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

        public char[,] GenerateTileMap(int width, int height)
        {
            Type = MapType.Generated;
            
            walkableTiles.Clear();
            nonWalkableTiles.Clear();
            exitTiles.Clear();

            int rows = width;
            int cols = height;

            

            //2d array to store values from the file (lines)
            char[,] tileArray = new char[rows, cols];

            Vector2 exitSpawnPoint = SpawnRandomExitTile(tileArray);

            //looping through lines and adding each char to the array in the correct position
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == exitSpawnPoint.X && j == exitSpawnPoint.Y)
                    {
                        tileArray[i, j] = 'E';
                    }
                    
                    //drawing a box.s
                    else if (i == 0 || i == rows - 1 || j == 0 || j == cols - 1)
                    {
                        tileArray[i, j] = '1';
                    }
                    else
                    {                        
                        tileArray[i, j] = RandomizeTile('1', '0');
                    }
                }
            }

            //returning a 2D array that will be used to generate(draw) the tilemap
            return tileArray;
        }

        //method to map the numbers inside a csv to Tiles in my dictionary using their keys (TileType)
        private Tile ConverCharToTile(char tileValue)
        {            
            Tile.TileType tileType = tileValue switch
            {
                '0' => Tile.TileType.Grass, // grass
                '1' => Tile.TileType.Dirt, // dirt
                'E' => Tile.TileType.Exit, // exit
                _ => Tile.TileType.Border // default
            };
            return tileData[tileType];
        }               

        /// <summary>
        /// Generates a map and caches it as a char[,] variable inside the TileManager Class
        /// </summary>
        public void CacheMapData(bool generate)
        {
            if(generate)
            {
                currentMap = GenerateTileMap(10, 10);
            }
            else
            {                
                currentMap = LoadMapFromFile(maps[MapIndex]);
                Console.WriteLine($"Map {MapIndex} Loaded. Walkable Tiles Count: {walkableTiles.Count}");
            }                   
        }

        //method to draw each tile from a given 2D character array in the correct size and position
        public void Draw(SpriteBatch spriteBatch)
        {                                                       
            //looping through each row and column
            for (int row = 0; row < currentMap.GetLength(0); row++)
            {
                for (int col = 0; col < currentMap.GetLength(1); col++)
                {                    
                    //converting the current value in the row/column of the array to a Tile 
                    char tileValue = currentMap[row, col];
                    Tile tile = ConverCharToTile(tileValue);
                                                                                
                    //determing the position to write the tiles at using their dimensions and the position in the array               
                    Vector2 position = new Vector2(col * tileWidth, row * tileHeight);

                    CacheTilePosition(tile, position);
                    
                    //Debug.WriteLine(sourceRect);
                    spriteBatch.Draw(tileAtlas, position, tile.SourceRect, Color.White);
                }
            }
        }
        
        public void CacheTilePosition(Tile tile, Vector2 position)
        {
            //tracking walkable tiles
            if (tile.Status == Tile.TileStatus.NotWalkable && !nonWalkableTiles.Contains(position))
            {
                nonWalkableTiles.Add(position);               
            }

            //tracking valid spawn points
            if (tile.Status == Tile.TileStatus.isWalkable && !walkableTiles.Contains(position) && tile.Type != Tile.TileType.Exit)
            {
                walkableTiles.Add(position);               
            }

            //tracking the exit tiles
            if (tile.Type == Tile.TileType.Exit)
            {
                exitTiles.Add(position);                
            }

            
        }

        /// <summary>
        /// 25% chance to return char1
        /// </summary>
        /// <param name="char1"></param>
        /// <param name="char2"></param>
        /// <returns></returns>
        private char RandomizeTile(char char1, char char2)
        {
            Random random = new Random();

            int randomChoice = random.Next(0, 4);

            if(randomChoice == 1)
            {
                return char1;
            }
            return char2;
        }

        private Vector2 SpawnRandomExitTile(char[,] map)
        {
            Random random = new();

            Vector2 position = new(random.Next(0, map.GetLength(0) - 1), random.Next(1, map.GetLength(1) - 1));
                       
            return position;
        }
    }
}
