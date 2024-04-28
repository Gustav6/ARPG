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

        public void GenerateMap()
        {
            Chunks = new List<Chunk>
            {
                new Chunk(ChunkId.plain)
            };

            // Procedural generation for a map 
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
