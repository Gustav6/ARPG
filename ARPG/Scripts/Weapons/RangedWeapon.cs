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

        public void ShootProjectile(ProjectileType type, Vector2 projectileDirection)
        {
            Projectile projectile = new(type, spawnPosition, damageAmount, speedOfProjectile, 2)
            {
                ownerOfProjectile = ownerOfWeapon,
                direction = projectileDirection,
            };

            Library.AddGameObject(projectile);
        }
    }
}
