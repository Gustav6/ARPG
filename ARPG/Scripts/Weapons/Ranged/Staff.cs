using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Staff : RangedWeapon
    {
        public Staff(Entity owner)
        {
            damageAmount = 50;
            speedOfProjectile = 800;
            ownerOfWeapon = owner;

            #region Draw variables
            texture = TextureManager.WeaponTexturesPairs[WeaponTextures.Staff];
            #endregion
        }

        public override void Attack()
        {
            spawnPosition = Position;
            spawnPosition.Y -= texture.Height / 2;

            ShootProjectile(ProjectileType.Fireball, DirectionTowardsMouse(spawnPosition));
        }
    }
}
