using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public static class UIManager
    {
        private static float frameRate;
        public static bool showFps = true;

        public static void Update(GameTime gameTime)
        {
            frameRate = MathF.Round(1 / (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            #region Draw user interface
            spriteBatch.Begin();

            if (showFps)
            {
                spriteBatch.DrawString(TextureManager.Font, "FPS : " + frameRate, new Vector2(75, 50), Color.Green, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, TextureManager.SpriteLayers[SpriteLayer.UI]);
                spriteBatch.DrawString(TextureManager.Font, "Health: " + Library.playerInstance.Health, new Vector2(75, 100), Color.Green, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, TextureManager.SpriteLayers[SpriteLayer.UI]);
                spriteBatch.DrawString(TextureManager.Font, Library.gameObjects.Count.ToString(), new Vector2(75, 150), Color.Green, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, TextureManager.SpriteLayers[SpriteLayer.UI]);
            }

            spriteBatch.End();
            #endregion
        }
    }
}
