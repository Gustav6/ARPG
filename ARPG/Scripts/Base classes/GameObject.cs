using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public abstract class GameObject : IRotatable
    {
        #region Draw variables
        public Texture2D Texture { get; protected set; }
        public Color color = Color.White;
        protected Vector2 origin;
        protected Vector2 scale = Vector2.One;
        protected SpriteEffects spriteEffects = SpriteEffects.None;

        protected float rotation;
        public float spriteLayer = TextureManager.SpriteLayers[SpriteLayer.Default];
        public Rectangle source;
        #endregion

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Position { private set; get; }
        public bool IsDestroyed { get; private set; }

        public virtual void CallOnInstantiate()
        {
            if (Texture != null)
            {
                origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
                source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            }
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public void Destroy()
        {
            IsDestroyed = true;
        }

        public virtual void CallOnDestroy() { }

        public virtual void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void SetRotation(float newRotation)
        {
            Rotation = newRotation;
        }

        public void FlipSprite(Vector2 direction)
        {
            switch (spriteEffects)
            {
                case SpriteEffects.None:

                    if (direction.X < 0)
                    {
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }

                    break;
                case SpriteEffects.FlipHorizontally:

                    if (direction.X > 0)
                    {
                        spriteEffects = SpriteEffects.None;
                    }

                    break;
                default:
                    break;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, source, color, rotation, origin, scale, spriteEffects, spriteLayer);
        }
    }
}
