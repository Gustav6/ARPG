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
        public bool CanMove {  get; private set; }

        public Vector2 direction;
        protected float speed;
        protected bool spriteCanFlip = true;

        private float addForceTimer;

        public override void CallOnEnable()
        {
            UnlockMovement();

            base.CallOnEnable();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void LockMovement()
        {
            CanMove = false;
        }

        public void UnlockMovement()
        {
            CanMove = true;
        }

        public void Move(GameTime gameTime)
        {
            if (direction == Vector2.Zero)
            {
                return;
            }

            direction.Normalize();

            if (spriteCanFlip)
            {
                FlipSprite(direction);
            }

            Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public Node GetNode(Rectangle hitbox)
        {
            Node node = null;

            if (Library.activeRoom != null)
            {
                for (int x = 0; x < Library.activeRoom.grid.GetLength(0); x++)
                {
                    for (int y = 0; y < Library.activeRoom.grid.GetLength(1); y++)
                    {
                        if (hitbox.Intersects(Library.activeRoom.grid[x, y].Hitbox))
                        {
                            node = Library.activeRoom.grid[x, y].node;
                        }
                    }
                }
            }

            return node;
        }
    }
}
