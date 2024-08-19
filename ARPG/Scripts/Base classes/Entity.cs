using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    #region Base state
    public abstract class EntityBaseState
    {
        public abstract void EnterState(Entity entityReference);
        public abstract void Update(GameTime gameTime);
        public abstract void ExitState();
    }
    #endregion

    public class Entity : Moveable, ICollidable
    {
        public EntityBaseState CurrentState { get; private set; }

        #region Hand variables
        public Vector2 handOffset;
        public Hand leftHand, rightHand;
        #endregion

        #region Hitbox variables

        private Rectangle hitbox;
        public Rectangle BoundingBox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        #endregion

        #region Health variables
        public bool Invincible { get; private set; }
        public float invincibleTime = 0.2f;

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

        public float knockBackDuration = 0.3f;

        public bool canAttack = true;
        public float attackSpeed = 5;

        public override void CallOnInstantiate()
        {
            #region Instansiate hands
            leftHand = new Hand(this);
            rightHand = new Hand(this);

            leftHand?.UpdatePosition(-handOffset);
            rightHand?.UpdatePosition(handOffset);
            #endregion

            base.CallOnInstantiate();
        }

        public override void Update(GameTime gameTime)
        {
            CurrentState.Update(gameTime);

            rightHand.weapon?.Update(gameTime);
            leftHand.weapon?.Update(gameTime);

            base.Update(gameTime);
        }

        #region Attack methods

        public virtual void Attack()
        {
            if (!canAttack)
                return;

            rightHand.weapon?.Attack();
            leftHand.weapon?.Attack();

            DisableAttack();

            Library.StartTimer(1 / attackSpeed, EnableAttack);
        }

        public void EnableAttack()
        {
            canAttack = true;
        }

        public void DisableAttack()
        {
            canAttack = false;
        }

        #endregion

        #region Position related methods

        public override void SetPosition(Vector2 newPosition)
        {
            base.SetPosition(newPosition);

            UpdateHitboxAndHands();
        }

        public void UpdateHitboxAndHands()
        {
            leftHand?.UpdatePosition(-handOffset);
            rightHand?.UpdatePosition(handOffset);
            hitbox.Location = new Vector2(Position.X, Position.Y).ToPoint();
        }

        #endregion

        #region Heatlh realated methods

        public virtual void OnDamage()
        {
            EnableInvincibility();

            Library.StartTimer(invincibleTime, RunAfterInvincibleFrames);
        }

        public virtual void RunAfterInvincibleFrames()
        {
            DisableInvincibility();
        }

        public void ApplyDamage(float damageAmount)
        {
            if (!Invincible)
            {
                Health -= damageAmount;

                OnDamage();
            }
        }

        public virtual void Heal(float healAmount)
        {
            Health += healAmount;
        }

        public void EnableInvincibility()
        {
            Invincible = true;
        }

        public void DisableInvincibility()
        {
            Invincible = false;
        }
        #endregion

        #region Knockback related methods
        public void ApplyKnockback(float knockbackStrength, Vector2 knockBackDirection)
        {
            direction = knockbackStrength * knockBackDirection;

            Library.StartTimer(knockBackDuration, RunAfterKnockBack);
        }

        public virtual void RunAfterKnockBack() { }

        #endregion

        #region Switch state method
        public void SwitchState(EntityBaseState state)
        {
            CurrentState?.ExitState();

            CurrentState = state;

            CurrentState.EnterState(this);
        }
        #endregion

        public virtual void OnCollision(ICollidable source) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            leftHand?.Draw(spriteBatch);
            rightHand?.Draw(spriteBatch);

            base.Draw(spriteBatch);

            //spriteBatch.Draw(texture, hitbox.Location.ToVector2(), source, Color.Black * 0.5f, Rotation, origin, scale, spriteEffects, spriteLayer + 0.1f);
        }
    }
}
