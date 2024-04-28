using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Chunk
    {
        public Tile[,] tiles;
        private List<Tile> tileList = new();

        public Chunk(ChunkId id)
        {
            tiles = new Tile[(int)TileMap.chunkSize, (int)TileMap.chunkSize];

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int xPosition = x * (int)TileMap.tileSize;
                    int yPosition = y * (int)TileMap.tileSize;

                    tiles[x, y] = (new Tile(TextureManager.TileTexturePairs[TileTextures.passable], new Vector2(xPosition, yPosition), new Rectangle(xPosition, yPosition, (int)TileMap.tileSize, (int)TileMap.tileSize), TileType.passable));

                    tileList.Add(tiles[x, y]);
                }
            }
        }

        public void DrawChunk(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tileList.Count; i++)
            {
                tileList[i].Draw(spriteBatch);
            }
        }
    }

    public enum ChunkId
    {
        plain
    }
}
