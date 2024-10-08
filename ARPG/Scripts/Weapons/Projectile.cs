﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Projectile : Moveable, ICollidable
    {
        public Entity ownerOfProjectile;
        private ProjectileType type;

        private float lifeSpan;
        private float damage;
        private float knockbackStrength;
        private float pierceTimes;

        private List<IDamageable> entitiesDamaged = new();

        private Rectangle hitbox;
        public Rectangle BoundingBox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        public Projectile(ProjectileType type, Vector2 startingPos, float _damage, float _speed, float _pierceTimes = 1, float knockback = 5, float _lifeSpan = 5)
        {
            #region Starting variables
            SetPosition(startingPos);
            lifeSpan = _lifeSpan;
            damage = _damage;
            speed = _speed;
            knockbackStrength = knockback;
            pierceTimes = _pierceTimes;
            #endregion

            #region Draw variables
            Texture = TextureManager.ProjectileTextures[type];
            spriteLayer = TextureManager.SpriteLayers[SpriteLayer.Projectile];
            #endregion

            this.type = type;

            hitbox = new Rectangle((int)base.Position.X, (int)base.Position.Y, Texture.Width, Texture.Height);
        }

        public override void CallOnInstantiate()
        {
            base.CallOnInstantiate();
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            SetHitboxPosition();

            if (lifeSpan <= 0)
            {
                Destroy();
            }
            else
            {
                lifeSpan -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        private void SetHitboxPosition()
        {
            hitbox.Location = new Vector2(Position.X, Position.Y).ToPoint();
        }

        public void OnCollision(ICollidable source)
        {
            if (source == ownerOfProjectile) 
                return;

            if (source is IDamageable d)
            {
                if (entitiesDamaged.Contains(d))
                {
                    return;
                }

                d.ApplyDamage(damage);
                d.ApplyKnockback(knockbackStrength, direction);
                entitiesDamaged.Add(d);
                pierceTimes--;

                if (pierceTimes <= 0)
                {
                    Destroy();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //spriteBatch.Draw(texture, hitbox.Location.ToVector2(), source, Color.Black * 0.5f, Rotation, origin, scale, spriteEffects, spriteLayer + 0.1f);
        }
    }

    public enum ProjectileType
    {
        Fireball,
    }
}
