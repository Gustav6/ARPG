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

        public Projectile(Vector2 _position, Vector2 _direction, Texture2D _texture, GameObject owner)
        {
            #region Starting variables
            Position = _position;
            direction = _direction;
            texture = _texture;
            ownerOfProjectile = owner;
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
