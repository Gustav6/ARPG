using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Player : Moveable
    {
        public Player(Vector2 startingPosition)
        {
            #region Starting variables
            Position = startingPosition;
            texture = TextureManager.TexturePairs[Textures.playerTexture];
            movementSpeed = 100;
            #endregion

            SetOriginAndSource(texture);

            Start();
        }

        public override void Update(GameTime gameTime)
        {
            Controls();

            base.Update(gameTime);
        }

        private void Controls()
        {
            direction = Vector2.Zero;

            #region Give Direction
            if (KeyboardInput.IsPressed(Keys.W) && !KeyboardInput.IsPressed(Keys.S))
                direction.Y = -1;

            if (KeyboardInput.IsPressed(Keys.S) && !KeyboardInput.IsPressed(Keys.W))
                direction.Y = 1;

            if (KeyboardInput.IsPressed(Keys.A) && !KeyboardInput.IsPressed(Keys.D))
                direction.X = -1;

            if (KeyboardInput.IsPressed(Keys.D) && !KeyboardInput.IsPressed(Keys.A))
                direction.X = 1;

            #endregion
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
