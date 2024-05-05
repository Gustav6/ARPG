using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public static class GameManager
    {
        public static bool showFps = true;

        public static void Initialize(GraphicsDevice graphics)
        {
            Library.playerInstance = new Player(new Vector2(0, 0));

            Library.cameraInstance = new Camera(graphics.Viewport, Library.playerInstance);

            Library.tileMap.GenerateNewMap();

            Library.gameObjects = new List<GameObject>()
            {
                { Library.playerInstance },
                { Library.cameraInstance }
            };
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            TextureManager.LoadTextures(content, graphicsDevice);
        }

        public static void Update(GameTime gameTime)
        {
            InputManager.GetInput();
            TransitionSystem.Update(gameTime);

            for (int i = 0; i < Library.gameObjects.Count; i++)
            {
                Library.gameObjects[i].Update(gameTime);
            }

            for (int i = Library.gameObjects.Count - 1; i >= 0; i--)
            {
                if (Library.gameObjects[i].IsDestroyed)
                {
                    Library.gameObjects[i].CallOnDestroy();
                    Library.gameObjects.RemoveAt(i);
                }
            }

            if (KeyboardInput.HasBeenPressed(Keys.Space))
            {
                //Library.tileMap.GenerateNewMap();

                Library.cameraInstance.ScreenShake(0.125f, 0.0085f);
            }

            if (KeyboardInput.IsPressed(Keys.F1))
            {
                Library.cameraInstance.Zoom -= 0.1f * Library.cameraInstance.Zoom;
            }
            if (KeyboardInput.IsPressed(Keys.F2))
            {
                Library.cameraInstance.Zoom += 0.1f * Library.cameraInstance.Zoom;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Library.tileMap.DrawMap(spriteBatch);

            for (int i = 0; i < Library.gameObjects.Count; i++)
            {
                Library.gameObjects[i].Draw(spriteBatch);
            }
        }
    }
}
