using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public static class TextureManager
    {
        private static GraphicsDevice graphicsDevice;

        public static readonly int tileSize = 64;
        public static SpriteFont Font { get; private set; }

        public static Dictionary <Textures, Texture2D> TexturePairs { get; private set; }
        public static Dictionary<TileTextures, Texture2D> TileTexturePairs { get; private set; }
        public static Dictionary<SpriteLayer, float> SpriteLayers { get; private set; }


        public static void LoadTextures(ContentManager content, GraphicsDevice _graphicsDevice)
        {
            graphicsDevice = _graphicsDevice;
            TexturePairs = new Dictionary<Textures, Texture2D>
            {
                //{ Textures.playerTexture, content.Load<Texture2D>("") },
                { Textures.playerTexture, CreateTexture(50, 100, pixel => Color.Blue) },
                { Textures.enemyTexture, CreateTexture(50, 100, pixel => Color.Red) }
            };

            TileTexturePairs = new Dictionary<TileTextures, Texture2D>
            {
                //{ TileTextures.passable, content.Load<Texture2D>("") },
                //{ TileTextures.unPassable, content.Load<Texture2D>("") }

                { TileTextures.passable, CreateTexture(tileSize, tileSize, pixel => Color.White) },
                { TileTextures.unPassable, CreateTexture(tileSize, tileSize, pixel => Color.Black) }
            };

            SpriteLayers = new Dictionary<SpriteLayer, float>
            {
                { SpriteLayer.Default, 0 },
                { SpriteLayer.Enemy, 0.1f },
                { SpriteLayer.Player, 0.2f },
            };

            Font = content.Load<SpriteFont>("spritefont");
        }

        public static Texture2D CreateTexture(int width, int height, Func<int, Color> paint)
        {
            Texture2D texture = new(graphicsDevice, width, height);

            Color[] colorArray = new Color[width * height];

            for (int pixel = 0; pixel < colorArray.Length; pixel++)
            {
                colorArray[pixel] = paint(pixel);
            }

            texture.SetData(colorArray);

            return texture;
        }

        public static Texture2D Hitbox(Vector2 size, Color color, float alpha)
        {
            return CreateTexture((int)size.X, (int)size.Y, pixel => color * alpha);
        }
    }

    public enum Textures
    {
        playerTexture,
        enemyTexture
    }
    public enum TileTextures
    {
        passable,
        unPassable,
    }

    public enum SpriteLayer
    {
        Default,    
        Enemy,
        Player,
    }
}
