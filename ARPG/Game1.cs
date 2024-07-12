using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata;

namespace ARPG
{
    public class Game1 : Game
    {
        private SpriteBatch spriteBatch;
        public static Game1 Reference { get; private set; }

        public Game1()
        {
            Reference = this;

            TextureManager.graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Library.windowHeight,
                PreferredBackBufferHeight = Library.windowWidth,
                //IsFullScreen = true,
            };

            Content.RootDirectory = "Content";
            Library.EnableCursor();
        }

        protected override void Initialize()
        {
            base.Initialize();

            GameManager.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.LoadTextures(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameManager.Update(gameTime);
            UIManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GameManager.Draw(spriteBatch);
            UIManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}