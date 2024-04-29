using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Projectile : Moveable
    {
        private GameObject ownerOfProjectile;
        private float lifeSpan;

        public Projectile(Texture2D texture, Vector2 position, Vector2 direction, float lifeSpan, GameObject owner)
        {
            #region Starting variables
            this.texture = texture;
            Position = position;
            this.direction = direction;
            this.lifeSpan = lifeSpan;
            ownerOfProjectile = owner;
            #endregion

            SetOriginAndSource(texture);

            Start();
        }

        public override void Update(GameTime gameTime)
        {
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
