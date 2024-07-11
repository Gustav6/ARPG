using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ARPG
{
    public static class RoomGeneration
    {
        #region Room generation variables
        private static readonly int minRoomWidth = 20, minRoomHeight = 20, maxRoomWidth = 75, maxRoomHeight = 75;

        private static int roomsLeft;
        private static readonly int maximumAmountOfRooms = 15;

        public static int minimumAmountOfEnemies = 5, maximumAmountOfEnemies = 15;
        #endregion


        public static void CallGeneration()
        {
            roomsLeft = maximumAmountOfRooms;
            GenerateRooms();
        }

        // To place corridors follow https://www.youtube.com/watch?v=rBY2Dzej03A&t=14s

        private static void GenerateRooms()
        {
            while (roomsLeft > 0)
            {
                #region Random variables
                int width = Library.rng.Next(minRoomWidth, maxRoomWidth);
                int height = Library.rng.Next(minRoomHeight, maxRoomHeight);
                int xPosition = Library.rng.Next(0, Library.tileMap.tileMapWidth - width);
                int yPosition = Library.rng.Next(0, Library.tileMap.tileMapHeight - height);
                #endregion

                int triesRemaining = 3;

                while (roomsLeft > 0 && triesRemaining > 0)
                {
                    #region Generate a new room

                    int amountOfEnemies = Library.rng.Next(minimumAmountOfEnemies, maximumAmountOfEnemies + 1);
                    Room currentRoom = new (width, height, xPosition, yPosition, amountOfEnemies);

                    #endregion

                    if (CanRoomBePlaced(xPosition, yPosition, currentRoom.grid))
                    {
                        PlaceRoom(xPosition, yPosition, currentRoom.grid);

                        if (Library.playerInstance != null && roomsLeft == maximumAmountOfRooms)
                        {
                            Vector2 newPlayerPos = new (xPosition + width / 2, yPosition + height / 2);
                            newPlayerPos *= TextureManager.tileSize;

                            Library.playerInstance.SetPosition(newPlayerPos);
                            Library.activeRoom = currentRoom;

                            for (int i = currentRoom.enemies.Count - 1; i >= 0; i--)
                            {
                                if (currentRoom.enemies[i].BoundingBox.Intersects(Library.playerInstance.BoundingBox))
                                {
                                    currentRoom.enemies[i].Destroy();
                                }
                            }

                            currentRoom.OnEnterRoom();
                        }

                        Library.tileMap.rooms.Add(currentRoom);

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

        private static void PlaceRoom(int xPosition, int yPosition, Tile[,] room)
        {
            for (int x = 0; x < room.GetLength(0); x++)
            {
                for (int y = 0; y < room.GetLength(1); y++)
                {
                    Library.tileMap.masterMap[x + xPosition, y + yPosition] = room[x, y];
                }
            }
        }
    }
}
