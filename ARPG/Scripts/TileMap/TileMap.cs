using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class TileMap
    {
        #region Constant variables
        public static int tileSize = 64;
        public List<Tile> tiles = new();
        public Tile[,] masterMap;
        private Tile[,] currentRoom;
        #endregion

        #region Room generation variables
        private int minRoomWidth = 20, minRoomHeight = 20, maxRoomWidth = 75, maxRoomHeight = 75;

        private int roomsLeft;
        private int maximumAmountOfRooms = 15;

        private List<Tile[,]> rooms = new();
        #endregion

        public int tileMapWidth = 350;
        public int tileMapHeight = 350;

        #region Room procedural generation

        public void GenerateNewMap()
        {
            tiles.Clear();
            masterMap = new Tile[tileMapWidth, tileMapHeight];

            #region Call room generation
            roomsLeft = maximumAmountOfRooms;
            PlaceRooms();
            #endregion
        }
        // To place corridors follow https://www.youtube.com/watch?v=rBY2Dzej03A&t=14s

        private void PlaceRooms()
        {
            while (roomsLeft > 0)
            {
                #region Random Variables
                int width = Library.rng.Next(minRoomWidth, maxRoomWidth);
                int height = Library.rng.Next(minRoomHeight, maxRoomHeight);
                int xPosition = Library.rng.Next(0, tileMapWidth - width);
                int yPosition = Library.rng.Next(0, tileMapHeight - height);
                #endregion

                int triesRemaining = 3;

                while (triesRemaining > 0 && roomsLeft > 0)
                {
                    Tile[,] currentRoom = GenerateARoom(width, height, xPosition, yPosition);

                    if (CanRoomBePlaced(xPosition, yPosition, currentRoom))
                    {
                        PlaceRoomOnTileMap(xPosition, yPosition, currentRoom);

                        if (Library.playerInstance != null && roomsLeft == maximumAmountOfRooms)
                        {
                            Vector2 newPlayerPos = new (xPosition + width / 2, yPosition + height / 2);
                            newPlayerPos *= tileSize;

                            Library.playerInstance.Position = newPlayerPos;
                        }

                        roomsLeft--;
                    }
                    else
                    {
                        triesRemaining--;
                    }
                }
            }
        }

        #region Room Generation
        private bool CanRoomBePlaced(int xPosition, int yPosition,Tile[,] room)
        {
            for (int x = 0; x < room.GetLength(0); x++)
            {
                for (int y = 0; y < room.GetLength(1); y++)
                {
                    if (masterMap[x + xPosition, y + yPosition] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void PlaceRoomOnTileMap(int xPosition, int yPosition, Tile[,] room)
        {
            for (int x = 0; x < room.GetLength(0); x++)
            {
                for (int y = 0; y < room.GetLength(1); y++)
                {
                    masterMap[x + xPosition, y + yPosition] = room[x, y];

                    tiles.Add(masterMap[x + xPosition, y + yPosition]);
                }
            }

            rooms.Add(room);
        }

        private Tile[,] GenerateARoom(int width, int height, int xPos, int yPos)
        {
            currentRoom = new Tile[width, height];

            for (int x = 0; x < currentRoom.GetLength(0); x++)
            {
                for (int y = 0; y < currentRoom.GetLength(1); y++)
                {
                    float xPosition = x * tileSize + xPos * tileSize;
                    float yPosition = y * tileSize + yPos * tileSize;

                    Texture2D texture = TextureManager.TileTexturePairs[TileTextures.passable];
                    TileType type = TileType.passable;

                    if (x == 0  || x == currentRoom.GetLength(0) - 1 || y == 0 || y == currentRoom.GetLength(1) - 1)
                    {
                        texture = TextureManager.TileTexturePairs[TileTextures.unPassable];
                        type = TileType.unPassable;
                    }

                    currentRoom[x, y] = new Tile(texture, new Vector2(xPosition, yPosition), new Rectangle((int)xPosition, (int)yPosition, tileSize, tileSize), type);
                }
            }

            return currentRoom;
        }
        #endregion

        #endregion

        #region Perlin noise generation

        public void Map()
        {
            GenerateNoiseMap(64, 64);
        }

        private void GenerateNoiseMap(int width, int height)
        {
            //noiseTiles = new NoiseTile[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float randomNum = (float)Library.rng.NextDouble();

                    TileTextures type;

                    if (randomNum > 0.75f)
                    {
                        type = TileTextures.unPassable;
                    }
                    else
                    {
                        type = TileTextures.passable;
                    }

                    //noiseTiles[x, y] = new NoiseTile(new Vector2(x * tileSize, y * tileSize), type);
                }
            }

            // Might help
            //noiseTiles = GameOfLife.Smoothing(noiseTiles);
        }

        private Vector2 RandomVector(Vector2 corner)
        {
            return new Vector2(Library.rng.Next(-1, 1) + corner.X, Library.rng.Next(-1, 1) + corner.Y);
        }

        private float PerlinNoise(float x, float y)
        {

            return 1;
        }

        #endregion


        public void DrawMap(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch);
            }

            //for (int x = 0; x < noiseTiles.GetLength(0); x++)
            //{
            //    for (int y = 0; y < noiseTiles.GetLength(1); y++)
            //    {
            //        noiseTiles[x, y].Draw(spriteBatch);
            //    }
            //}
        }
    }
}
