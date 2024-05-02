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
            spriteLayer = (int)SpriteLayer.Player;
            movementSpeed = 800;
            #endregion

            SetOriginAndSource(texture);

            Start();
        }

        public override void Update(GameTime gameTime)
        {
            MovementInput();

            base.Update(gameTime);
        }

        private void MovementInput()
        {
            direction = new Vector2(KeyboardInput.Horizontal(), KeyboardInput.Vertical());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private enum PlayerState
        {
            Idle,
            Walking,
            Attacking,
        }
    }
}
