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
        #region Constant variables
        public List<Tile> tiles = new();
        public Tile[,] masterMap;
        #endregion

        public readonly int tileMapWidth = 350;
        public readonly int tileMapHeight = 350;

        public void GenerateNewMap()
        {
            tiles.Clear();
            masterMap = new Tile[tileMapWidth, tileMapHeight];

            GenerateRooms.CallGeneration();
        }

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


        public void DrawMap(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch);
            }

            //for (int x = 0; x < noiseTiles.GetLength(0); x++)
            //{
            //    for (int y = 0; y < noiseTiles.GetLength(1); y++)
            //    {
            //        noiseTiles[x, y].Draw(spriteBatch);
            //    }
            //}
        }
    }
}
