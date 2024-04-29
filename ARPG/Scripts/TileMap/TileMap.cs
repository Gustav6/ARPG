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
        public static float tileSize = 64;
        public static float chunkSize = 32;
        public List<Chunk> Chunks { get; private set; }

        public void GenerateChunks()
        {
            Chunks = new List<Chunk>
            {
                new Chunk(ChunkId.plain, new Vector2((chunkSize * tileSize) / -2, (chunkSize * tileSize) / -2))
            };

            // Procedural generation 
        }

        public void DrawChunks(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Chunks.Count; i++)
            {
                Chunks[i].DrawChunk(spriteBatch);
            }
        }
    }
}
