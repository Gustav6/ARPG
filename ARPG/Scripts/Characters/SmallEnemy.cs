﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class SmallEnemy : Enemy
    {
        public SmallEnemy(Vector2 startingPosition)
        {
            #region Starting variables
            Position = startingPosition;
            speed = 300;
            maxHealth = 50;
            health = maxHealth;

            handOffset = new Vector2(24, 0);
            #endregion

            #region Draw variables
            texture = TextureManager.EntityTexturesPairs[EntityTextures.SmallEnemy];
            spriteLayer = TextureManager.SpriteLayers[SpriteLayer.Enemy];
            #endregion

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            feetHitbox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, 10);
            feetHitboxOffset = texture.Height / 2 + feetHitbox.Height;
        }

        public override void CallOnEnable()
        {
            base.CallOnEnable();

            leftHand.scale = new Vector2(0.5f, 0.5f);
            rightHand.scale = new Vector2(0.5f, 0.5f);
        }
    }
}
