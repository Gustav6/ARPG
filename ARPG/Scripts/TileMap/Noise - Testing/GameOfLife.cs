using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    internal class GameOfLife
    {
        public static NoiseTile[,] SetNeighbors(NoiseTile[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y].amountOfNeighbors = 0;

                    if (InBounds(x + 1, y, map.GetLength(0), map.GetLength(1)))
                    {
                        if (map[x + 1, y].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                        {
                            map[x, y].amountOfNeighbors++;
                        }
                    }
                    if (InBounds(x - 1, y, map.GetLength(0), map.GetLength(1)))
                    {
                        if (map[x - 1, y].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                        {
                            map[x, y].amountOfNeighbors++;
                        }
                    }
                    if (InBounds(x + 1, y + 1, map.GetLength(0), map.GetLength(1)))
                    {
                        if (map[x + 1, y + 1].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                        {
                            map[x, y].amountOfNeighbors++;
                        }
                    }
                    if (InBounds(x - 1, y + 1, map.GetLength(0), map.GetLength(1)))
                    {
                        if (map[x - 1, y + 1].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                        {
                            map[x, y].amountOfNeighbors++;
                        }
                    }
                    if (InBounds(x, y + 1, map.GetLength(0), map.GetLength(1)))
                    {
                        if (map[x, y + 1].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                        {
                            map[x, y].amountOfNeighbors++;
                        }
                    }
                    if (InBounds(x, y - 1, map.GetLength(0), map.GetLength(1)))
                    {
                        if (map[x, y - 1].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                        {
                            map[x, y].amountOfNeighbors++;
                        }
                    }
                    if (InBounds(x + 1, y - 1, map.GetLength(0), map.GetLength(1)))
                    {
                        if (map[x + 1, y - 1].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                        {
                            map[x, y].amountOfNeighbors++;
                        }
                    }
                    if (InBounds(x - 1, y - 1, map.GetLength(0), map.GetLength(1)))
                    {
                        if (map[x - 1, y - 1].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                        {
                            map[x, y].amountOfNeighbors++;
                        }
                    }
                }
            }

            return map;
        }

        public static NoiseTile[,] Smoothing(NoiseTile[,] map)
        {
            SetNeighbors(map);

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    //if (map[x, y].texture == TextureManager.TileTexturePairs[TileTextures.passable])
                    if (map[x, y].texture == TextureManager.TileTexturePairs[TileTextures.unPassable])
                    {
                        if (map[x, y].amountOfNeighbors < 2)
                        {
                            //map[x, y].texture = TextureManager.TileTexturePairs[TileTextures.unPassable];

                            map[x, y].texture = TextureManager.TileTexturePairs[TileTextures.passable];
                        }
                        if (map[x, y].amountOfNeighbors == 2 || map[x, y].amountOfNeighbors == 3)
                        {
                            //map[x, y].texture = TextureManager.TileTexturePairs[TileTextures.passable];

                            map[x, y].texture = TextureManager.TileTexturePairs[TileTextures.unPassable];
                        }
                        if (map[x, y].amountOfNeighbors > 3)
                        {
                            //map[x, y].texture = TextureManager.TileTexturePairs[TileTextures.unPassable];

                            map[x, y].texture = TextureManager.TileTexturePairs[TileTextures.passable];
                        }
                    }
                    //else if (map[x, y].texture == TextureManager.TileTexturePairs[TileTextures.unPassable] && map[x, y].amountOfNeighbors == 3)
                    else if (map[x, y].texture == TextureManager.TileTexturePairs[TileTextures.passable] && map[x, y].amountOfNeighbors == 3)
                    {
                        //map[x, y].texture = TextureManager.TileTexturePairs[TileTextures.passable];

                        map[x, y].texture = TextureManager.TileTexturePairs[TileTextures.unPassable];
                    }
                }
            }

            return map;
        }

        private static bool InBounds(int x, int y, int width, int height)
        {
            return 0 <= y && y < height && 0 <= x && x < width;
        }
    }
}
