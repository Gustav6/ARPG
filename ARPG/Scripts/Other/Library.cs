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
        public static Player playerInstance;

        public static List<GameObject> gameObjects = new();

        public static TileMap tileMap = new();
    }
}
