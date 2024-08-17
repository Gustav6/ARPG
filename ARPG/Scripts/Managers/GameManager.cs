using ARPG.Scripts.Managers;
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
        public static void Initialize()
        {
            Library.playerInstance = new Player(new Vector2(0, 0));

            Library.cameraInstance = new Camera(TextureManager.graphicsDeviceManager.GraphicsDevice.Viewport, Library.playerInstance);

            Library.AddGameObject(Library.playerInstance);

            Library.tileMap.GenerateNewMap();
        }

        public static void Update(GameTime gameTime)
        {
            InputManager.GetInput();
            TransitionSystem.Update(gameTime);
            CollisionManager.Update();

            Library.cameraInstance?.Update();

            for (int i = 0; i < Library.timers.Count; i++)
            {
                Library.timers[i].Update(gameTime);
            }

            #region Loops for game objects
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
            #endregion

            #region Checks for active room
            if (Library.activeRoom != null)
            {
                if (!Library.playerInstance.BoundingBox.Intersects(Library.activeRoom.bounds))
                {
                    if (!Library.activeRoom.hasExitedRoom)
                    {
                        Library.activeRoom.OnExitRoom();
                    }

                    for (int i = 0; i < Library.tileMap.rooms.Count; i++)
                    {
                        if (Library.playerInstance.BoundingBox.Intersects(Library.tileMap.rooms[i].bounds))
                        {
                            Library.activeRoom = Library.tileMap.rooms[i];
                            Library.activeRoom.OnEnterRoom();
                            break;
                        }
                    }
                }
                else if (Library.activeRoom.hasExitedRoom)
                {
                    Library.activeRoom.OnEnterRoom();
                }
            }
            else
            {
                for (int i = 0; i < Library.tileMap.rooms.Count; i++)
                {
                    if (Library.playerInstance.BoundingBox.Intersects(Library.tileMap.rooms[i].bounds))
                    {
                        Library.activeRoom = Library.tileMap.rooms[i];
                        Library.activeRoom.OnEnterRoom();
                        break;
                    }
                }
            }
            #endregion

            #region Debug

            if (KeyboardInput.HasBeenPressed(Keys.Space))
            {
                Library.tileMap.GenerateNewMap();
            }

            if (KeyboardInput.IsPressed(Keys.F1))
            {
                Library.cameraInstance.Zoom -= 0.1f * Library.cameraInstance.Zoom;
            }
            if (KeyboardInput.IsPressed(Keys.F2))
            {
                Library.cameraInstance.Zoom += 0.1f * Library.cameraInstance.Zoom;
            }
            #endregion
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            #region Draw world
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Library.cameraInstance.Transform);

            Library.tileMap.DrawMap(spriteBatch);

            if (Library.activeRoom != null)
            {
                Library.activeRoom.Draw(spriteBatch);
            }

            for (int i = 0; i < Library.gameObjects.Count; i++)
            {
                Library.gameObjects[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            #endregion
        }
    }
}
