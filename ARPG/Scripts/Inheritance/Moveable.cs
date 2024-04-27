using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public abstract class Moveable : GameObject
    {
        public bool canMove;

        protected Vector2 direction;
        protected float movementSpeed;

        public override void Start()
        {
            canMove = true;

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            if (canMove)
            {
                Move(gameTime);
            }

            base.Update(gameTime);
        }

        protected void Move(GameTime gameTime)
        {
            if (direction != Vector2.Zero)
            {
                direction.Normalize();

                Position += direction * movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
