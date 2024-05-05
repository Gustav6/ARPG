using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class NoiseTile
    {
        public Texture2D texture;
        public Vector2 gradient0, gradient1, gradient2, gradient3;
        public Vector2 position;
        public Color color = Color.White;

        public int amountOfNeighbors = 0;

        public NoiseTile(Vector2 _position, TileTextures type)
        {
            position = _position;
            texture = TextureManager.TileTexturePairs[type];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
