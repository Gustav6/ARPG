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

        public int tileMapWidth = 250;
        public int tileMapHeight = 250;

        private int minRoomWidth = 15, minRoomHeight = 15, maxRoomWidth = 75, maxRoomHeight = 75;
        private int roomsXPosition, roomsYPosition, roomsWidth, roomsHeight;

        private int roomsLeft;
        private int maximumAmountOfRooms = 15;

        public List<Chunk> Chunks { get; private set; }

        public void GenerateNewMap()
        {
            tiles.Clear();
            roomsLeft = maximumAmountOfRooms;

            masterMap = new Tile[tileMapWidth, tileMapHeight];

            PlaceRooms();
        }

        private void PlaceRooms()
        {
            while (roomsLeft > 0)
            {
                Tile[,] currentRoom = GenerateNewRoom();

                int triesRemaining = 3;

                while (triesRemaining > 0 && roomsLeft > 0)
                {
                    if (CanRoomBePlaced(currentRoom))
                    {
                        PlaceRoomOnTileMap(currentRoom);

                        if (Library.playerInstance != null && roomsLeft == maximumAmountOfRooms)
                        {
                            Vector2 newPlayerPos = new (roomsXPosition + roomsWidth / 2, roomsYPosition + roomsHeight / 2);
                            newPlayerPos *= tileSize;

                            Library.playerInstance.Position = newPlayerPos;
                        }

                        roomsLeft--;
                    }
                    else
                    {
                        currentRoom = GenerateNewRoom();

                        triesRemaining--;
                    }
                }
            }
        }
        private bool CanRoomBePlaced(Tile[,] testRoom)
        {
            for (int x = 0; x < testRoom.GetLength(0); x++)
            {
                for (int y = 0; y < testRoom.GetLength(1); y++)
                {
                    if (masterMap[x + roomsXPosition, y + roomsYPosition] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void PlaceRoomOnTileMap(Tile[,] room)
        {
            for (int x = 0; x < room.GetLength(0); x++)
            {
                for (int y = 0; y < room.GetLength(1); y++)
                {
                    masterMap[x + roomsXPosition, y + roomsYPosition] = room[x, y];

                    tiles.Add(masterMap[x + roomsXPosition, y + roomsYPosition]);
                }
            }
        }

        private Tile[,] GenerateNewRoom()
        {
            roomsWidth = Library.rng.Next(minRoomWidth, maxRoomWidth);
            roomsHeight = Library.rng.Next(minRoomHeight, maxRoomHeight);
            roomsXPosition = Library.rng.Next(0, tileMapWidth - roomsWidth);
            roomsYPosition = Library.rng.Next(0, tileMapHeight - roomsHeight);

            return CreateRoom(roomsWidth, roomsHeight, roomsXPosition, roomsYPosition);
        }

        private Tile[,] CreateRoom(int width, int height, int xPos, int yPos)
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
                        type = TileType.unpassable;
                    }

                    currentRoom[x, y] = new Tile(texture, new Vector2(xPosition, yPosition), new Rectangle((int)xPosition, (int)yPosition, tileSize, tileSize), type);
                }
            }

            return currentRoom;
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch);
            }
        }
    }
}
