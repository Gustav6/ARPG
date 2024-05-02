using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Enemy : Moveable
    {
        public Enemy(Vector2 startingPosition)
        {
            #region Starting variables
            Position = startingPosition;
            texture = TextureManager.TexturePairs[Textures.enemyTexture];
            spriteLayer = (int)SpriteLayer.Enemy;
            movementSpeed = 100;
            #endregion

            SetOriginAndSource(texture);

            Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
