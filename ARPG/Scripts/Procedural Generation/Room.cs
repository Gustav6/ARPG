using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Room
    {
        public Vector2 Position { get; private set; }

        public Tile[,] grid;
        private int width, height;
        private int x, y;

        public Rectangle bounds;

        public List<Enemy> enemies = [];
        public int amountOfEnemies;

        public bool hasExitedRoom = false;

        public Room(int width, int height, int xPos, int yPos, int amountOfEnemies)
        {
            Position = new Vector2(xPos * TextureManager.tileSize, yPos * TextureManager.tileSize);

            this.width = width;
            this.height = height;
            x = xPos;
            y = yPos;

            grid = new Tile[width, height];

            #region Creating the room
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float xPosition = x * TextureManager.tileSize + xPos * TextureManager.tileSize;
                    float yPosition = y * TextureManager.tileSize + yPos * TextureManager.tileSize;
                    Vector2 position = new (xPosition, yPosition);

                    Texture2D texture = TextureManager.TileTexturePairs[TileTextures.passable];
                    TileType type = TileType.passable;

                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        texture = TextureManager.TileTexturePairs[TileTextures.unPassable];
                        type = TileType.unPassable;
                    }

                    grid[x, y] = new Tile(texture, position, type, x, y);
                }
            }
            #endregion

            this.amountOfEnemies = amountOfEnemies;

            SpawnEnemies();

            bounds = new Rectangle((int)Position.X, (int)Position.Y, width * TextureManager.tileSize, height * TextureManager.tileSize);
        }

        #region Spawn enemies
        public void SpawnEnemies()
        {
            for (int i = 0; i < amountOfEnemies; i++)
            {
                float randomXPosition = Library.rng.Next((int)Position.X + TextureManager.tileSize, (width - 1) * TextureManager.tileSize + x * TextureManager.tileSize);
                float randomYPosition = Library.rng.Next((int)Position.Y + TextureManager.tileSize, (height - 1) * TextureManager.tileSize + y * TextureManager.tileSize);

                Vector2 spawnPosition = new(randomXPosition, randomYPosition);

                EnemyType enemyType = (EnemyType)Library.rng.Next(0, Enum.GetNames(typeof(EnemyType)).Length + 1);

                Enemy enemyToSpawn = enemyType switch
                {
                    EnemyType.Small => new SmallEnemy(spawnPosition),
                    EnemyType.Large => new LargeEnemy(spawnPosition),
                    _ => new SmallEnemy(spawnPosition),
                };

                enemies.Add(enemyToSpawn);
            }
        }
        #endregion

        #region Run when player enters room
        public void OnEnterRoom()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (!enemies[i].IsDestroyed)
                {
                    Library.AddGameObject(enemies[i]);
                }
                else
                {
                    enemies.RemoveAt(i);
                }
            }

            hasExitedRoom = false;
        }
        #endregion

        #region Run when player exits room
        public void OnExitRoom()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (!enemies[i].IsDestroyed)
                {
                    Library.gameObjects.Remove(enemies[i]);
                }
            }

            hasExitedRoom = true;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y].Draw(spriteBatch);
                }
            }
        }
    }
}
