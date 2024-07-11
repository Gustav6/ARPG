using Microsoft.Xna.Framework;

namespace ARPG
{
    public abstract class Weapon : GameObject
    {
        public Entity ownerOfWeapon;
        public float damageAmount;
        
        public override void CallOnEnable()
        {
            base.CallOnEnable();

            spriteLayer = TextureManager.SpriteLayers[SpriteLayer.Weapon];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public abstract void Attack();

        public void SetPosition()
        {
            Position = ownerOfWeapon.rightHand.position;
        }

        public static void Create(WeaponID weaponType, Hand hand, Entity owner)
        {
            switch (weaponType)
            {
                case WeaponID.Staff:
                    hand.weapon = new Staff(owner);
                    break;
                case WeaponID.Sword: 
                    break;
                default:
                    break;
            }

            hand.weapon.CallOnEnable();
        }
    }

    public enum WeaponID
    {
        // Ranged
        Staff,

        // Melee
        Sword,
    }
}
