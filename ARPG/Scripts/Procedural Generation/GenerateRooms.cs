using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ARPG
{
    public static class GenerateRooms
    {
        #region Room generation variables
        private static readonly int minRoomWidth = 20, minRoomHeight = 20, maxRoomWidth = 75, maxRoomHeight = 75;

        private static int roomsLeft;
        private static readonly int maximumAmountOfRooms = 15;

        private static List<Tile[,]> rooms = new();
        private static Tile[,] currentRoom;
        private static Tile[,] tempRoom;
        #endregion


        public static void CallGeneration()
        {
            roomsLeft = maximumAmountOfRooms;
            PlaceRooms();
        }

        // To place corridors follow https://www.youtube.com/watch?v=rBY2Dzej03A&t=14s

        private static void PlaceRooms()
        {
            while (roomsLeft > 0)
            {
                #region Random Variables
                int width = Library.rng.Next(minRoomWidth, maxRoomWidth);
                int height = Library.rng.Next(minRoomHeight, maxRoomHeight);
                int xPosition = Library.rng.Next(0, Library.tileMap.tileMapWidth - width);
                int yPosition = Library.rng.Next(0, Library.tileMap.tileMapHeight - height);
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
                            Vector2 newPlayerPos = new(xPosition + width / 2, yPosition + height / 2);
                            newPlayerPos *= TextureManager.tileSize;

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
        private static bool CanRoomBePlaced(int xPosition, int yPosition, Tile[,] room)
        {
            for (int x = 0; x < room.GetLength(0); x++)
            {
                for (int y = 0; y < room.GetLength(1); y++)
                {
                    if (Library.tileMap.masterMap[x + xPosition, y + yPosition] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static void PlaceRoomOnTileMap(int xPosition, int yPosition, Tile[,] room)
        {
            for (int x = 0; x < room.GetLength(0); x++)
            {
                for (int y = 0; y < room.GetLength(1); y++)
                {
                    Library.tileMap.masterMap[x + xPosition, y + yPosition] = room[x, y];

                    Library.tileMap.tiles.Add(Library.tileMap.masterMap[x + xPosition, y + yPosition]);
                }
            }

            rooms.Add(room);
        }


        #region Room Generation
        private static Tile[,] GenerateARoom(int width, int height, int xPos, int yPos)
        {
            tempRoom = new Tile[width, height];

            for (int x = 0; x < tempRoom.GetLength(0); x++)
            {
                for (int y = 0; y < tempRoom.GetLength(1); y++)
                {
                    float xPosition = x * TextureManager.tileSize + xPos * TextureManager.tileSize;
                    float yPosition = y * TextureManager.tileSize + yPos * TextureManager.tileSize;

                    Texture2D texture = TextureManager.TileTexturePairs[TileTextures.passable];
                    TileType type = TileType.passable;

                    if (x == 0 || x == tempRoom.GetLength(0) - 1 || y == 0 || y == tempRoom.GetLength(1) - 1)
                    {
                        texture = TextureManager.TileTexturePairs[TileTextures.unPassable];
                        type = TileType.unPassable;
                    }

                    tempRoom[x, y] = new Tile(texture, new Vector2(xPosition, yPosition), new Rectangle((int)xPosition, (int)yPosition, TextureManager.tileSize, TextureManager.tileSize), type);
                }
            }

            return tempRoom;
        }
        #endregion
    }
}
