using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public static class GameManager
    {
        public static void Initialize()
        {
            Library.tileMap.GenerateMap();

            Library.playerInstance = new Player(new Vector2(500, 100));

            Library.gameObjects.Add(Library.playerInstance);
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            TextureManager.LoadTextures(content, graphicsDevice);
        }

        public static void Update(GameTime gameTime)
        {
            InputController.GetInput();

            for (int i = 0; i < Library.gameObjects.Count; i++)
            {
                Library.gameObjects[i].Update(gameTime);
            }

            for (int i = Library.gameObjects.Count - 1; i >= 0; i--)
            {
                if (Library.gameObjects[i].IsDestroyed)
                {
                    Library.gameObjects.RemoveAt(i);
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Library.gameObjects.Count; i++)
            {
                Library.gameObjects[i].Draw(spriteBatch);
            }

            Library.tileMap.DrawMap(spriteBatch);
        }
    }
}
