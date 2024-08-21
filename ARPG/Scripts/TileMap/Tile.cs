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
    public class Tile(Texture2D texture, Vector2 position, TileType type)
    {
        #region Draw variables
        private readonly Texture2D texture = texture;
        public Color color = Color.White;
        #endregion

        public readonly Rectangle hitbox = new((int)position.X, (int)position.Y, TextureManager.tileSize, TextureManager.tileSize);
        public readonly Vector2 position = position;
        public readonly TileType type = type;

        private Rectangle source = new(0, 0, texture.Width, texture.Height);
        private Vector2 origin = new(texture.Width / 2, texture.Height / 2);

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, source, color, 0, origin, Vector2.One, SpriteEffects.None, TextureManager.SpriteLayers[SpriteLayer.Default]);

            //node.DebugDraw(spriteBatch);
        }
    }

    public enum TileType
    {
        passable, 
        unPassable,
        empty
    }
}
