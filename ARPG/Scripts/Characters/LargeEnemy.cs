using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class LargeEnemy : Enemy
    {
        public LargeEnemy(Vector2 startingPosition)
        {
            #region Starting variables
            SetPosition(startingPosition);
            speed = 200;
            maxHealth = 200;
            Health = maxHealth;

            handOffset = new Vector2(48, 0);
            #endregion

            #region Draw variables
            texture = TextureManager.EntityTexturesPairs[EntityTextures.LargeEnemy];
            spriteLayer = TextureManager.SpriteLayers[SpriteLayer.Enemy];
            #endregion

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            feetHitbox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, 10);
            feetHitboxOffset = texture.Height / 2 + feetHitbox.Height;
        }
    }
}
