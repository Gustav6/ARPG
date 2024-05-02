using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public abstract class GameObject
    {
        #region Draw variables
        protected Texture2D texture;
        protected Color color = Color.White;
        protected Vector2 scale = Vector2.One;
        protected Vector2 origin;
        protected float spriteLayer = (int)SpriteLayer.Default;
        public float rotation = 0;
        public Rectangle source;
        #endregion

        public Vector2 Position { get; set; }
        public bool IsDestroyed { get; private set; }

        public virtual void Start()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public void Destroy()
        {
            IsDestroyed = true;
        }

        public virtual void CallOnDestroy()
        {

        }

        protected void SetOriginAndSource(Texture2D _texture)
        {
            origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            source = new Rectangle(0, 0, _texture.Width, _texture.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, source, color, rotation, origin, scale, SpriteEffects.None, spriteLayer);
        }
    }
}
