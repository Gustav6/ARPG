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
        public Vector2 Position { get; private set; }
        public Tile[,] tileMap;
        private List<Tile> tiles = new();

        public Rectangle chunkHitbox;
        //private Point hitboxSize = new((int)(TileMap.chunkSize * TileMap.tileSize), (int)(TileMap.chunkSize * TileMap.tileSize));

        public Chunk(ChunkId id, Vector2 position)
        {
            //tileMap = new Tile[(int)TileMap.chunkSize, (int)TileMap.chunkSize];
            Position = position;

            for (int x = 0; x < tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < tileMap.GetLength(1); y++)
                {
                    int xPosition = (x * TileMap.tileSize) + (int)position.X;
                    int yPosition = (y * TileMap.tileSize) + (int)position.Y;

                    Texture2D texture = TextureManager.TileTexturePairs[TileTextures.passable];

                    switch (id)
                    {
                        case ChunkId.plain:
                            texture = TextureManager.TileTexturePairs[TileTextures.passable];
                            break;
                        case ChunkId.beach:
                            texture = TextureManager.TileTexturePairs[TileTextures.passable];
                            break;
                        default:
                            break;
                    }

                    tileMap[x, y] = (new Tile(texture, new Vector2(xPosition, yPosition), 
                        new Rectangle(xPosition, yPosition, TileMap.tileSize, TileMap.tileSize), 
                        TileType.passable));

                    tiles.Add(tileMap[x, y]);
                }
            }

            //chunkHitbox = new Rectangle(position.ToPoint(), hitboxSize);
        }

        public void DrawChunk(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch);
            }
        }
    }

    public enum ChunkId
    {
        plain,
        beach,
        ocean,
    }
}
