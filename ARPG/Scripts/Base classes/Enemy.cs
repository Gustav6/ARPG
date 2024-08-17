using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static ARPG.ActionTimer;

namespace ARPG
{
    public abstract class Enemy : Entity, IDamageable
    {
        #region States
        public EnemyIdleState idleState = new();
        public EnemyMovingState movingState = new();
        public EnemyHurtState hurtState = new();
        #endregion

        private readonly float contactDamage = 1;
        private readonly float knockbackStrength = 100;
        public bool canPathFind = true;

        public List<Node> prevPath;
        public List<Node> path;

        public override void CallOnInstantiate()
        {
            SwitchState(idleState);

            Library.playerInstance.OnNodeChange += PlayerInstance_OnNodeChange;

            base.CallOnInstantiate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnDamage()
        {
            base.OnDamage();

            canPathFind = false;

            SwitchState(hurtState);
        }

        public override void RunAfterInvincibleFrames()
        {
            base.RunAfterInvincibleFrames();

            canPathFind = true;
        }

        public override void RunAfterKnockBack()
        {
            base.RunAfterKnockBack();

            if (HasReachedTarget())
            {
                direction = Vector2.Zero;
                SwitchState(idleState);
            }
            else
            {
                SwitchState(movingState);
            }
        }

        #region Methods related to pathfinding
        public void PlayerInstance_OnNodeChange(object sender, EventArgs e)
        {
            if (CanMove && Library.playerInstance != null)
            {
                FindPath();
            }
        }

        public void FindPath()
        {
            Node? startingNode = GetNode(feetHitbox, Library.activeRoom), targetNode = Library.playerInstance.currentNode;

            if (startingNode != null && targetNode != null)
            {
                path = Library.AStarManager.FindPath(Library.activeRoom.grid, startingNode.Value, targetNode.Value);

                if (CurrentState != movingState && path.Count > 0)
                {
                    SwitchState(movingState);
                }
                else if (CurrentState != idleState && path.Count == 0)
                {
                    SwitchState(idleState);
                }
            }
        }

        public bool HasValidPath()
        {
            if (path != null && path.Count > 0)
            {
                return true;
            }
            else if (CurrentState == movingState)
            {
                direction = Vector2.Zero;
            }

            return false;
        }

        public bool HasReachedTarget()
        {
            if (HasValidPath())
            {
                if (BoundingBox.Intersects(path.Last().Hitbox))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        public override void OnCollision(ICollidable source)
        {
            base.OnCollision(source);

            #region Collision with player
            if (source is Player p)
            {
                if (!p.Invincible)
                {
                    p.ApplyDamage(contactDamage);

                    if (!p.IsDestroyed)
                    {
                        Vector2 knockbackDirection;

                        if (direction != Vector2.Zero)
                        {
                            knockbackDirection = direction;
                        }
                        else if (p.direction != Vector2.Zero)
                        {
                            knockbackDirection = -p.direction;
                        }
                        else
                        {
                            knockbackDirection = Position - p.Position;

                            if (knockbackDirection != Vector2.Zero)
                            {
                                knockbackDirection.Normalize();
                            }
                            else
                            {
                                knockbackDirection = Vector2.One;
                            }
                        }

                        p.ApplyKnockback(knockbackStrength, knockbackDirection);
                    }
                }
            }
            #endregion
        }

        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
            // Draw health "variable"

            if (Health > 0)
            {
                float currentHealth = Health / maxHealth;

                Color color = Color.Green;

                if (currentHealth <= 0.5f && currentHealth > 0.25f)
                {
                    color = Color.Yellow;
                }
                else if (currentHealth <= 0.25f)
                {
                    color = Color.Red;
                }

                Vector2 size = TextureManager.Font.MeasureString(Health.ToString());

                Vector2 position = new(Position.X - size.X / 2, Position.Y + texture.Height / 2);

                spriteBatch.DrawString(TextureManager.Font, Health.ToString(), position, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, TextureManager.SpriteLayers[SpriteLayer.UI]);
            }

            // Draw health background


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawHealthBar(spriteBatch);
        }
    }

    #region Idle state
    public class EnemyIdleState : EntityBaseState
    {
        private Enemy enemy;

        public override void EnterState(Entity enemyReference)
        {
            enemy = (Enemy)enemyReference;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void ExitState()
        {

        }
    }
    #endregion

    #region Moving state
    public class EnemyMovingState : EntityBaseState
    {
        private Enemy enemy;
        private Node? nextTarget;

        public override void EnterState(Entity enemyReference)
        {
            enemy = (Enemy)enemyReference;

            if (enemy.canPathFind && enemy.HasValidPath())
            {
                FollowPath();
            }
        }

        public override void Update(GameTime gameTime)
        {
            enemy.Move(gameTime);
            enemy.UpdateHitboxAndHands();

            //PathFind();

            if (enemy.canPathFind && nextTarget != null)
            {
                if (enemy.path.Count > 0)
                {
                    if (enemy.HasReachedTarget())
                    {
                        enemy.direction = Vector2.Zero;
                    }
                    else if (enemy.BoundingBox.Intersects(nextTarget.Value.Hitbox))
                    {
                        enemy.path.RemoveAt(0);
                        FollowPath();
                    }
                }
                else
                {
                    enemy.SwitchState(enemy.idleState);
                }
            }

            if (!enemy.CanMove || enemy.direction == Vector2.Zero)
            {
                enemy.SwitchState(enemy.idleState);
            }
        }

        private void FollowPath()
        {
            if (enemy.path.Count == 0)
            {
                return;
            }

            nextTarget = enemy.path.First();
            enemy.direction = DirectionTowardsNode(nextTarget.Value);
        }

        private Vector2 DirectionTowardsNode(Node target)
        {
            Vector2 result = target.Position - enemy.feetHitbox.Location.ToVector2();

            result.Normalize();

            return result;
        }

        public override void ExitState()
        {
            nextTarget = null;
        }
    }
    #endregion

    #region HurtState
    public class EnemyHurtState : EntityBaseState
    {
        private Enemy enemy;

        public override void EnterState(Entity enemyReference)
        {
            enemy = (Enemy)enemyReference;

            if (enemy.Health <= 0)
            {
                enemy.Destroy();
            }
            else
            {
                // Switch state to moving because of knockback

                enemy.SwitchState(enemy.movingState);
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

    public enum EnemyType
    {
        Small,
        Large,
    }
}
