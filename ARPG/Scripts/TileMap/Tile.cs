using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Tile
    {
        #region Draw variables
        private Texture2D texture;
        private Color color = Color.White;
        #endregion

        public Rectangle Hitbox { get; private set; }
        public Vector2 Position { get; private set; }
        public TileType Type { get; private set; }

        public Tile(Texture2D _texture, Vector2 _position, Rectangle hitbox, TileType type)
        {
            texture = _texture;
            Position = _position;
            Type = type;
            Hitbox = hitbox;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, color);

            // Draw a hitbox over tiles that cant be passed

            if (Type == TileType.impassable)
            {
                spriteBatch.Draw(TextureManager.Hitbox(new Vector2(Hitbox.Width, Hitbox.Height), Color.Green, 1), Position, Color.White);
            }
        }
    }

    public enum TileType
    {
        passable, 
        impassable
    }
}
