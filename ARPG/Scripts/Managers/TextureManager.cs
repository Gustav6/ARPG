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
        public static GraphicsDeviceManager graphicsDeviceManager;

        public static readonly int tileSize = 64;
        public static SpriteFont Font { get; private set; }
        public static Texture2D Hitbox { get; private set; }

        public static Dictionary <EntityTextures, Texture2D> EntityTexturesPairs { get; private set; }
        public static Dictionary<WeaponTextures, Texture2D> WeaponTexturesPairs { get; private set; }
        public static Dictionary<TileTextures, Texture2D> TileTexturePairs { get; private set; }
        public static Dictionary<ProjectileType, Texture2D> ProjectileTextures { get; private set; }
        public static Dictionary<SpriteLayer, float> SpriteLayers { get; private set; }


        public static void LoadTextures(ContentManager content)
        {
            EntityTexturesPairs = new Dictionary<EntityTextures, Texture2D>
            {
                //{ Textures.playerTexture, content.Load<Texture2D>("") },
                { EntityTextures.Player, CreateTexture(48, 96, pixel => Color.Blue) },
                { EntityTextures.SmallEnemy, CreateTexture(32, 64, pixel => Color.Red) },
                { EntityTextures.LargeEnemy, CreateTexture(64, 128, pixel => Color.Red) },
                { EntityTextures.Hand, CreateTexture(32, 32, pixel => Color.Blue) }
            };

            WeaponTexturesPairs = new Dictionary<WeaponTextures, Texture2D>
            {
                { WeaponTextures.Staff,  CreateTexture(24, 128, pixel => Color.Red)}
            };

            TileTexturePairs = new Dictionary<TileTextures, Texture2D>
            {
                //{ TileTextures.passable, content.Load<Texture2D>("") },
                //{ TileTextures.unPassable, content.Load<Texture2D>("") }

                { TileTextures.passable, CreateTexture(tileSize, tileSize, pixel => Color.White) },
                { TileTextures.unPassable, CreateTexture(tileSize, tileSize, pixel => Color.Gray) }
            };

            ProjectileTextures = new Dictionary<ProjectileType, Texture2D>
            {
                { ProjectileType.Fireball, CreateTexture(32, 32, Pixel => Color.Red) },
            };


            SpriteLayers = new Dictionary<SpriteLayer, float>
            {
                { SpriteLayer.Default, 0 },
                { SpriteLayer.Projectile, 0.1f },
                { SpriteLayer.Enemy, 0.2f },
                { SpriteLayer.Player, 0.3f },
                { SpriteLayer.Weapon, 0.4f },
                { SpriteLayer.UI, 0.5f },
            };

            Font = content.Load<SpriteFont>("spritefont");

            Hitbox = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
            Hitbox.SetData(new Color[] { Color.White });
        }

        public static Texture2D CreateTexture(int width, int height, Func<int, Color> paint)
        {
            Texture2D texture = new(graphicsDeviceManager.GraphicsDevice, width, height);

            Color[] colorArray = new Color[width * height];

            for (int pixel = 0; pixel < colorArray.Length; pixel++)
            {
                colorArray[pixel] = paint(pixel);
            }

            texture.SetData(colorArray);

            return texture;
        }
    }

    public enum EntityTextures
    {
        Player,
        SmallEnemy,
        LargeEnemy,
        Hand
    }

    public enum WeaponTextures
    {
        Staff,
    }

    public enum TileTextures
    {
        passable,
        unPassable,
    }

    public enum SpriteLayer
    {
        Default,   
        Projectile,
        Enemy,
        Player,
        Weapon,
        UI,
    }
}
