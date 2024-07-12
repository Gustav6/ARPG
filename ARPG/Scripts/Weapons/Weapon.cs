using Microsoft.Xna.Framework;

namespace ARPG
{
    public abstract class Weapon : GameObject
    {
        public Entity ownerOfWeapon;
        public float damageAmount;
        
        public override void CallOnEnable()
        {
            spriteLayer = TextureManager.SpriteLayers[SpriteLayer.Weapon];

            base.CallOnEnable();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public static void SpawnWeapon(WeaponID weaponType, Hand hand, Entity owner)
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

        public void UpdatePosition(Vector2 handsPosition)
        {
            SetPosition(handsPosition);
        }

        public abstract void Attack();

        protected Vector2 DirectionTowardsMouse(Vector2 originFromAttack)
        {
            if (originFromAttack == Vector2.Zero)
            {
                originFromAttack = Library.playerInstance.Position;
            }

            Vector2 finalDirection = Library.cameraInstance.ScreenToWorldSpace() - originFromAttack;

            if (finalDirection != Vector2.Zero)
            {
                finalDirection.Normalize();
            }
            else
            {
                finalDirection = new Vector2(0, 1);
            }

            return finalDirection;
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
