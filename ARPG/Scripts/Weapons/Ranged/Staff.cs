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
            damageAmount = 1;
            speedOfProjectile = 800;
            texture = TextureManager.WeaponTexturesPairs[WeaponTextures.Staff];
            ownerOfWeapon = owner;
        }

        public override void CallOnEnable()
        {
            texture = TextureManager.WeaponTexturesPairs[WeaponTextures.Staff];

            base.CallOnEnable();
        }

        public override void Attack()
        {
            spawnPosition = Position;
            spawnPosition.Y -= texture.Height / 2;

            ShootProjectile(ProjectileType.Fireball, DirectionTowardsMouse());
        }
    }
}
