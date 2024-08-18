using Microsoft.Xna.Framework;
using System;

namespace ARPG
{
    public class Player : Entity, IDamageable
    {
        #region State variables
        public PlayerIdleState idleState = new();
        public PlayerMovingState movingState = new();
        public PlayerHurtState hurtState = new();
        #endregion

        public Node? currentNode;
        public event EventHandler OnNodeChange;

        public Player(Vector2 startingPosition)
        {
            #region Starting variables
            SetPosition(startingPosition);
            speed = 600;
            Health = maxHealth;

            handOffset = new Vector2(48, 0);
            #endregion

            #region Draw variables
            texture = TextureManager.EntityTexturesPairs[EntityTextures.Player];
            spriteLayer = TextureManager.SpriteLayers[SpriteLayer.Player];
            #endregion

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            feetHitbox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, 10);
            feetHitboxOffset = texture.Height / 2 + feetHitbox.Height;
        }

        public override void CallOnInstantiate()
        {
            SwitchState(idleState);

            base.CallOnInstantiate();

            Weapon.SpawnWeapon(WeaponID.Staff, rightHand, this);
        }

        public override void Update(GameTime gameTime)
        {
            if (MouseInput.IsPressed(MouseInput.currentState.LeftButton))
            {
                Attack();
            }

            base.Update(gameTime);
        }

        public override void OnDamage()
        {
            base.OnDamage();

            SwitchState(hurtState);
        }

        public void CheckForNodeChange()
        {
            if (currentNode == null || !feetHitbox.Intersects(currentNode.Hitbox))
            {
                currentNode = GetNode(feetHitbox, Library.activeRoom);

                OnNodeChange?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    #region Idle state
    public class PlayerIdleState : EntityBaseState
    {
        private Player player;

        public override void EnterState(Entity playerReference)
        {
            player = (Player)playerReference;
        }

        public override void Update(GameTime gameTime)
        {
            if (player.CanMove)
            {
                if (KeyboardInput.Horizontal() != 0 || KeyboardInput.Vertical() != 0)
                {
                    player.SwitchState(player.movingState);
                }
            }
        }

        public override void ExitState()
        {

        }
    }
    #endregion

    #region Moving state
    public class PlayerMovingState : EntityBaseState
    {
        private Player player;

        public override void EnterState(Entity playerReference)
        {
            player = (Player)playerReference;
        }

        public override void Update(GameTime gameTime)
        {
            player.direction = new Vector2(KeyboardInput.Horizontal(), KeyboardInput.Vertical());

            player.Move(gameTime);
            player.UpdateHitboxAndHands();
            player.CheckForNodeChange();

            if (!player.CanMove || player.direction == Vector2.Zero)
            {
                player.SwitchState(player.idleState);
            }
        }

        public override void ExitState()
        {

        }
    }
    #endregion

    #region HurtState
    public class PlayerHurtState : EntityBaseState
    {
        private Player player;

        public override void EnterState(Entity playerReference)
        {
            player = (Player)playerReference;

            if (player.Health <= 0)
            {
                player.Destroy();
            }
            else
            {
                Library.cameraInstance.ScreenShake(player.knockBackDuration, 0.025f);

                player.SwitchState(player.movingState);
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void ExitState()
        {

        }
    }
    #endregion
}
