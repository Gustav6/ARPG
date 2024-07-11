using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    #region Base state
    public abstract class PlayerBaseState
    {
        public abstract void EnterState(Player playerReference);
        public abstract void Update(GameTime gameTime);
        public abstract void ExitState();
    }
    #endregion

    public class Player : Entity, IDamageable
    {
        #region State variables
        private PlayerBaseState currentState;

        public PlayerIdleState idleState;
        public PlayerWalkingState walkingState;
        public PlayerHurtState hurtState;
        #endregion

        #region Health variables

        public bool isDead = false;
        public float maxHealth = 200;
        private float health;
        public float Health
        {
            get
            {
                return health;
            }

            set
            {
                if (value < maxHealth)
                {
                    health = value;
                }
                else
                {
                    health = maxHealth;
                }
            }
        }
        #endregion

        public Tile tile;
        public event EventHandler OnTileChange;

        public Player(Vector2 startingPosition)
        {
            #region Starting variables
            Position = startingPosition;
            speed = 600;
            health = maxHealth;

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

        public override void CallOnEnable()
        {
            #region Setting state references
            idleState = new();
            walkingState = new();
            hurtState = new();
            #endregion

            SwitchState(idleState);

            base.CallOnEnable();

            Weapon.Create(WeaponID.Staff, rightHand, this);
        }

        public override void Update(GameTime gameTime)
        {
            currentState.Update(gameTime);

            if (direction != Vector2.Zero)
            {
                CheckForTileChange();
            }

            if (MouseInput.HasBeenPressed(MouseInput.currentState.LeftButton, MouseInput.prevState.LeftButton))
            {
                rightHand.weapon.Attack();
            }

            base.Update(gameTime);
        }

        #region Switch state code
        public void SwitchState(PlayerBaseState state)
        {
            currentState?.ExitState();

            currentState = state;

            currentState.EnterState(this);
        }
        #endregion

        #region Heatlh realated code
        public void ApplyDamage(float damageAmount)
        {
            if (!invincible)
            {
                health -= damageAmount;

                SwitchState(hurtState);
            }
        }

        public void ApplyKnockback(float knockbackStrength, Vector2 direction)
        {
            Knockback(knockbackStrength, direction);
        }

        public void Heal(float healAmount)
        {
            health += healAmount;
        }
        #endregion

        public void UpdatePathfinding()
        {
            OnTileChange?.Invoke(this, EventArgs.Empty);

            tile.color = Color.Cyan;
        }

        private void CheckForTileChange()
        {
            if (tile == null || !feetHitbox.Intersects(tile.Hitbox))
            {
                if (GetNode(feetHitbox) != null)
                {
                    if (tile != null)
                    {
                        tile.color = Color.White;
                    }
                    tile = GetNode(feetHitbox).ownerOfNode;

                    UpdatePathfinding();
                }
            }
        }
    }

    #region Idle state
    public class PlayerIdleState : PlayerBaseState
    {
        private Player player;

        public override void EnterState(Player playerReference)
        {
            player = playerReference;
        }

        public override void Update(GameTime gameTime)
        {
            if (player.CanMove)
            {
                if (KeyboardInput.Horizontal() != 0 || KeyboardInput.Vertical() != 0)
                {
                    player.SwitchState(player.walkingState);
                }
            }
        }

        public override void ExitState()
        {

        }
    }
    #endregion

    #region Walking state
    public class PlayerWalkingState : PlayerBaseState
    {
        private Player player;

        public override void EnterState(Player playerReference)
        {
            player = playerReference;
        }

        public override void Update(GameTime gameTime)
        {
            player.direction = new Vector2(KeyboardInput.Horizontal(), KeyboardInput.Vertical());

            player.Move(gameTime);
            player.SetHitboxPosition();

            if (player.direction == Vector2.Zero || !player.CanMove)
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
    public class PlayerHurtState : PlayerBaseState
    {
        private Player player;

        public override void EnterState(Player playerReference)
        {
            player = playerReference;

            if (player.Health <= 0)
            {
                player.Destroy();
            }

            player.invincible = true;

            Library.StartTimer(player.knockBackDuration, RunAfterKnockBack);

            Library.cameraInstance.ScreenShake(player.knockBackDuration, 0.025f);
        }

        public override void Update(GameTime gameTime)
        {
            player.Move(gameTime);
            player.SetHitboxPosition();
        }

        private void RunAfterKnockBack()
        {
            player.UnlockMovement();
            player.SwitchState(player.idleState);
            player.invincible = false;
            player.direction = Vector2.Zero;
        }

        public override void ExitState()
        {

        }
    }
    #endregion
}
