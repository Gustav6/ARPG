using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class NoiseGeneration
    {
        #region Perlin noise generation

        public void Map()
        {
            GenerateNoiseMap(64, 64);
        }

        private void GenerateNoiseMap(int width, int height)
        {
            //noiseTiles = new NoiseTile[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float randomNum = (float)Library.rng.NextDouble();

                    //TileTextures type;

                    if (randomNum > 0.75f)
                    {
                        //type = TileTextures.unPassable;
                    }
                    else
                    {
                        //type = TileTextures.passable;
                    }

                    //noiseTiles[x, y] = new NoiseTile(new Vector2(x * tileSize, y * tileSize), type);
                }
            }

            // Might help
            //noiseTiles = GameOfLife.Smoothing(noiseTiles);
        }

        private Vector2 RandomVector(Vector2 corner)
        {
            return new Vector2(Library.rng.Next(-1, 1) + corner.X, Library.rng.Next(-1, 1) + corner.Y);
        }

        private float PerlinNoise(float x, float y)
        {

            return 1;
        }

        #endregion
    }
}
