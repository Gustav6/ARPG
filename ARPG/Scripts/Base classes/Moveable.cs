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
        protected bool canFlip = true;

        public override void CallOnInstantiate()
        {
            UnlockMovement();

            base.CallOnInstantiate();
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

            if (canFlip)
            {
                FlipSprite(direction);
            }

            SetPosition(Position + direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public static Node? GetNode(Rectangle hitbox, Room room)
        {
            Node? node = null;

            if (room != null)
            {
                for (int x = 0; x < room.grid.GetLength(0); x++)
                {
                    for (int y = 0; y < room.grid.GetLength(1); y++)
                    {
                        if (hitbox.Intersects(room.grid[x, y].Hitbox))
                        {
                            node = room.grid[x, y].node;
                        }
                    }
                }
            }

            return node;
        }
    }
}
