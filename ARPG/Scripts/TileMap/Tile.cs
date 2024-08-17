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
    public class Tile
    {
        #region Draw variables
        private Texture2D texture;
        public Color color = Color.White;
        #endregion

        public Rectangle Hitbox { get; private set; }
        public Vector2 Position { get; private set; }
        public TileType Type { get; private set; }

        public Node node;

        private Rectangle source;
        private Vector2 origin;

        public Tile(Texture2D _texture, Vector2 _position, TileType type, int x, int y)
        {
            texture = _texture;
            Position = _position;
            Type = type;
            Hitbox = new Rectangle((int)_position.X, (int)_position.Y, TextureManager.tileSize, TextureManager.tileSize);

            bool walkable = false;

            if (type == TileType.passable)
            {
                walkable = true;
            }

            node = new Node(Position, new Point(x, y), walkable, this);

            if (texture != null)
            {
                origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
                source = new Rectangle(0, 0, _texture.Width, _texture.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, source, color, 0, origin, Vector2.One, SpriteEffects.None, TextureManager.SpriteLayers[SpriteLayer.Default]);

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
