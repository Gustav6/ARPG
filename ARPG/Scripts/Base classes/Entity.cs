using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Entity : Moveable, ICollidable
    {
        #region Hand variables
        public Vector2 handOffset;
        public Hand leftHand, rightHand;
        #endregion

        #region Hitbox variables

        public Rectangle feetHitbox;
        public int feetHitboxOffset;

        private Rectangle hitbox;
        public Rectangle BoundingBox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }
        #endregion

        public float knockBackDuration = 0.3f;
        public bool invincible;

        public override void CallOnEnable()
        {
            #region Create hands
            leftHand = new Hand(this);
            rightHand = new Hand(this);
            #endregion

            UpdateHandPosition();

            base.CallOnEnable();
        }

        public override void Update(GameTime gameTime)
        {
            rightHand.weapon?.Update(gameTime);
            leftHand.weapon?.Update(gameTime);

            if (direction != Vector2.Zero)
            {
                UpdateHandPosition();
            }

            base.Update(gameTime);
        }

        #region Update positions
        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
            UpdateHandPosition();
            SetHitboxPosition();
        }

        public void UpdateHandPosition()
        {
            leftHand?.SetPosition(-handOffset);
            rightHand?.SetPosition(handOffset);
        }

        public void SetHitboxPosition()
        {
            hitbox.Location = new Vector2(Position.X, Position.Y).ToPoint();
            feetHitbox.Location = new Point(hitbox.Location.X, hitbox.Location.Y + feetHitboxOffset);
        }
        #endregion

        protected void Knockback(float strength, Vector2 knockbackDirection)
        {
            direction = knockbackDirection * strength;
            LockMovement();
        }

        public virtual void OnCollision(ICollidable source) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            leftHand?.Draw(spriteBatch);
            rightHand?.Draw(spriteBatch);

            base.Draw(spriteBatch);

            //spriteBatch.Draw(texture, hitbox.Location.ToVector2(), source, Color.Black * 0.5f, Rotation, origin, scale, spriteEffects, spriteLayer + 0.1f);
        }
    }
}
