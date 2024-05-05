using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ARPG
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = Library.windowWidth,
                PreferredBackBufferWidth = Library.windowHeight,
                //IsFullScreen = true,
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            GameManager.Initialize(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameManager.LoadContent(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Library.cameraInstance.Transform);

            if (GameManager.showFps)
            {
                float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 position = new (Library.cameraInstance.X - 900, Library.cameraInstance.Y - 500);

                _spriteBatch.DrawString(TextureManager.Font, frameRate.ToString(), position, Color.Green, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, (int)SpriteLayer.GUI);
            }

            GameManager.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}