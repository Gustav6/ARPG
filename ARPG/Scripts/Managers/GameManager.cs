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
        public static void Initialize(GraphicsDevice graphics)
        {
            Library.tileMap.GenerateChunks();

            Library.playerInstance = new Player(new Vector2(0, 0));

            Library.gameObjects.Add(Library.playerInstance);

            Library.cameraInstance = new Camera(graphics.Viewport, Library.playerInstance);
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            TextureManager.LoadTextures(content, graphicsDevice);
        }

        public static void Update(GameTime gameTime)
        {
            InputController.GetInput();
            Library.cameraInstance.Update();
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

            if (KeyboardInput.IsPressed(Keys.Space))
            {
                //TransitionSystem.SINTransition(3, Library.playerInstance, 1, 1, 2);
            }

            if (KeyboardInput.IsPressed(Keys.F1))
            {
                Library.cameraInstance.Zoom -= 0.1f;
            }
            if (KeyboardInput.IsPressed(Keys.F2))
            {
                Library.cameraInstance.Zoom += 0.1f;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Library.tileMap.DrawChunks(spriteBatch);

            for (int i = 0; i < Library.gameObjects.Count; i++)
            {
                Library.gameObjects[i].Draw(spriteBatch);
            }
        }
    }
}
