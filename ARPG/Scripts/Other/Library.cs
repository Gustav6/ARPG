using ARPG.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ARPG.ActionTimer;

namespace ARPG
{
    public static class Library
    {
        public static List<ActionTimer> timers = new();

        public static int windowWidth = 1080;
        public static int windowHeight = 1920;

        #region Rotate Variables
        public static readonly float rotate45Degrees = MathF.PI / 4;
        public static readonly float rotate90Degrees = MathF.PI / 2;
        public static readonly float rotate180Degrees = MathF.PI;
        public static readonly float rotate270Degrees = MathF.PI * 1.5f;
        public static readonly float rotate360Degrees = MathF.PI * 2;
        #endregion

        public static Random rng = new();

        public static Player playerInstance;

        public static List<GameObject> gameObjects = new ();

        public static TileMap tileMap = new();

        public static Room activeRoom;

        public static Camera cameraInstance;

        public static void StartTimer(float duration, TimerCallbackDelegate callback)
        {
            ActionTimer timer = new();

            timer.SetTimer(duration, callback);
        }

        public static void AddGameObject(GameObject obj)
        {
            obj.CallOnEnable();
            gameObjects.Add(obj);
        }

        public static void EnableCursor() => Game1.Reference.IsMouseVisible = true;

        public static void DisableCursor() => Game1.Reference.IsMouseVisible = false;

        public static List<Tile> GetNeighbors(int positionX, int positionY, Tile[,] grid)
        {
            List<Tile> result = new();
            Tile NeighboringNode;

            #region Get neighbors
            if (InBounds(grid, positionX + 1, positionY))
            {
                NeighboringNode = grid[positionX + 1, positionY];

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX - 1, positionY))
            {
                NeighboringNode = grid[positionX - 1, positionY];

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX, positionY + 1))
            {
                NeighboringNode = grid[positionX, positionY + 1];

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX, positionY - 1))
            {
                NeighboringNode = grid[positionX, positionY - 1];

                result.Add(NeighboringNode);
            }

            if (InBounds(grid, positionX + 1, positionY + 1))
            {
                NeighboringNode = grid[positionX + 1, positionY + 1];

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX + 1, positionY - 1))
            {
                NeighboringNode = grid[positionX + 1, positionY - 1];

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX - 1, positionY - 1))
            {
                NeighboringNode = grid[positionX - 1, positionY - 1];

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX - 1, positionY + 1))
            {
                NeighboringNode = grid[positionX - 1, positionY + 1];

                result.Add(NeighboringNode);
            }
            #endregion

            return result;
        }

        private static bool InBounds(Tile[,] map, int x, int y)
        {
            return 0 <= y && y < map.GetLength(1) && 0 <= x && x < map.GetLength(0);
        }
    }
}
