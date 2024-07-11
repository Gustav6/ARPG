using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public abstract class RangedWeapon : Weapon
    {
        protected float speedOfProjectile = 300;
        protected Vector2 spawnPosition;

        public void ShootProjectile(ProjectileType type, Vector2 direction)
        {
            Projectile projectile = new(type, spawnPosition, damageAmount, speedOfProjectile)
            {
                ownerOfProjectile = ownerOfWeapon,
                direction = direction,
            };

            Library.AddGameObject(projectile);
        }

        public Vector2 DirectionTowardsMouse()
        {
            Vector2 finalDirection = Library.cameraInstance.ScreenToWorldSpace() - spawnPosition;

            if (finalDirection != Vector2.Zero)
            {
                finalDirection.Normalize();
            }
            else
            {
                finalDirection = new Vector2 (0, 1);
            }

            return finalDirection;
        }
    }
}
