using ARPG.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public static class Library
    {
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

        public static List<GameObject> gameObjects;

        public static TileMap tileMap = new();

        public static Camera cameraInstance;
    }
}
