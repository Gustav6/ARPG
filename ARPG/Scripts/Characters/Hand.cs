using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace ARPG
{
    public class Hand
    {
        #region Draw variables
        private Vector2 origin;
        private Texture2D texture;
        private Rectangle source;
        public Vector2 scale = Vector2.One;
        #endregion

        private Entity ownerOfHand;
        public Vector2 position;
        public Weapon weapon;

        public Hand(Entity owner)
        {
            texture = TextureManager.EntityTexturesPairs[EntityTextures.Hand];
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            source = new Rectangle(0, 0, texture.Width, texture.Height);
            ownerOfHand = owner;
        }

        public void SetPosition(Vector2 offset)
        {
            position = ownerOfHand.Position + offset;
            weapon?.SetPosition();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, source, Color.White, 0, origin, scale, SpriteEffects.None, ownerOfHand.spriteLayer);

            weapon?.Draw(spriteBatch);
        }
    }
}
