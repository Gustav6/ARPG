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
        public List<Tile> Tiles { get; private set; }

        public void GenerateMap()
        {
            Tiles = new List<Tile>();

            // Procedural generation for a map 
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                Tiles[i].Draw(spriteBatch);
            }
        }
    }
}
