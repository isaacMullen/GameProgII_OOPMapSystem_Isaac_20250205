using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameProgII_OOPMapSystem_Isaac_20250205
{
    internal class Player : GameEntity
    {
        //wil help with movement logic and checking for valid tiles
        private bool isMoving;
        Vector2 targetPosition;

        private KeyboardState previousKeyboardState;
        private TileManager tileManager;

        Random random = new Random();

        bool transitioningMaps = false;
        bool canRespawn = false;

        //takes a reference to TileManager so I can access the list with all of the NonWalkableTiles and compare my movement against them
        public Player(string name, Texture2D texture, TileManager tileManager, Vector2? startPosition = null) : base(name, texture) 
        {
            this.tileManager = tileManager;

            //Console.WriteLine(tileManager.walkableTiles.Count);
            
            //allowing for a starting position to be chosen if i need to. Plan to spawn the player randomly for the most part.
            if(startPosition.HasValue)
            {
                this.Position = startPosition.Value;
            }
            else
            {
                //setting the initial position to a random walkable tile (inside the walkable tiles list)
                this.Position = tileManager.walkableTiles[random.Next(0, tileManager.walkableTiles.Count - 1)];               
            }                        
        }

        public override void Update()
        {
            if(!canRespawn && tileManager.walkableTiles.Count > 0)
            {
                Console.WriteLine("Walkable tiles are now ready. Player can spawn.");
                canRespawn = true;
                Position = Respawn(tileManager.walkableTiles);
            }
            
            HandleInput();
            Move();
        }

        public override void Move()
        {           
            //final check to make sure i dont move if im already moving 
            if (isMoving)
            {               
                if (transitioningMaps && canRespawn)
                {
                    
                    Console.WriteLine($"Switching to Map {tileManager.MapIndex}");

                    // Print out the new map's walkable tiles
                    Console.WriteLine($"New Map Walkable Tiles Count: {tileManager.walkableTiles.Count}");

                    // Only respawn when tiles are available

                    //----------------------------ACTUALLY GENERATING THE MAP (BOOL FOR GENERATION OR LOADING OF FILES)----------------------------
                    tileManager.CacheMapData(true);
                    Position = Respawn(tileManager.walkableTiles);
                    transitioningMaps = false;
                    
                }


                //check if the tile is in the list of non walkable tiles
                if (!tileManager.nonWalkableTiles.Contains(targetPosition))
                {
                    Position = targetPosition; 
                }
                else
                {
                    Console.WriteLine("Invalid Move.");
                }
                isMoving = false;                 
            }
        }

        private void HandleInput()
        {
            if (isMoving) return;

            Vector2 inputDirection = Vector2.Zero;
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W) && !previousKeyboardState.IsKeyDown(Keys.W))
            {
                inputDirection.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.S) && !previousKeyboardState.IsKeyDown(Keys.S))
            {
                inputDirection.Y += 1;
            }
            if (keyboardState.IsKeyDown(Keys.A) && !previousKeyboardState.IsKeyDown(Keys.A))
            {
                inputDirection.X -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.D) && !previousKeyboardState.IsKeyDown(Keys.D))
            {
                inputDirection.X += 1;
            }

            // If there was any input, calculate the target position
            if (inputDirection != Vector2.Zero)
            {
                SetDirection(inputDirection);

                targetPosition = Position + Direction * GridSize;
                
                //only go to next map if it exists                
                if(tileManager.Type == TileManager.MapType.Loaded)
                {
                    if (tileManager.exitTiles.Contains(targetPosition) && tileManager.maps.Count - 1 >= tileManager.MapIndex + 1)
                    {                        
                        Console.WriteLine($"Walkable Tiles: {tileManager.walkableTiles.Count}");
                        transitioningMaps = true;                        
                        tileManager.MapIndex += 1;
                        
                    }
                }
                else if(tileManager.Type == TileManager.MapType.Generated)
                {
                    if (tileManager.exitTiles.Contains(targetPosition))
                    {
                        //setting a bool to notify the respawn method redraw the player when they exit the current map
                        transitioningMaps = true;                        
                    }
                }                                                                              
                isMoving = true;
            }

            // Store the current state for the next frame
            previousKeyboardState = keyboardState;
        }

        private void SetDirection(Vector2 newDirection)
        {
            if(newDirection != Vector2.Zero)
            {
                Direction = newDirection;
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        //using recursion to iterate over spaces that arent exit tiles so i dont accidentally spawn right overtop of one
        public Vector2 Respawn(List<Vector2> spawnPoints)
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                Console.WriteLine("Respawn called too early! Waiting for walkable tiles...");
                return Position; // Keep the player in the same position until tiles are ready
            }

            Random random = new();
            Vector2 position;

            //keeps checking for valid spawn position if the spawn position is the same as an exit tile (since theyre both walkable tiles)
            do
            {
                position = spawnPoints[random.Next(spawnPoints.Count)];
            } 
            while (tileManager.exitTiles.Contains(position));

            Console.WriteLine($"Player spawning at: {position}");
            return position;
        }
    }
}
