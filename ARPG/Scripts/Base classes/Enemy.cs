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
    #region Base state
    public abstract class EnemyBaseState
    {
        public abstract void EnterState(Enemy enemyReference);
        public abstract void Update(GameTime gameTime);
        public abstract void ExitState();
    }
    #endregion

    public abstract class Enemy : Entity, IDamageable
    {
        #region States
        private EnemyBaseState currentState;

        public EnemyIdleState idleState;
        public EnemyWalkingState walkingState;
        public EnemyHurtState hurtState;
        #endregion

        #region Health variables
        public bool isDead = false;

        protected float maxHealth;
        protected float health;
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

        private readonly float contactDamage = 1;
        private readonly float knockbackStrength = 100;
        public List<Node> path;

        public override void CallOnEnable()
        {
            #region Setting state references
            idleState = new();
            walkingState = new();
            hurtState = new();
            #endregion

            SwitchState(idleState);

            Library.playerInstance.OnTileChange += PlayerInstance_OnTileChange;

            base.CallOnEnable();
        }

        public override void Update(GameTime gameTime)
        {
            currentState.Update(gameTime);

            base.Update(gameTime);
        }

        #region Switch state code
        public void SwitchState(EnemyBaseState state)
        {
            currentState?.ExitState();

            currentState = state;

            currentState.EnterState(this);
        }
        #endregion

        #region Heatlh realated code
        public void ApplyDamage(float damageAmount)
        {
            health -= damageAmount;

            SwitchState(hurtState);
        }

        public void ApplyKnockback(float knockbackStrength, Vector2 direction)
        {
            Knockback(knockbackStrength, direction);
        }

        #endregion

        public void PlayerInstance_OnTileChange(object sender, EventArgs e)
        {
            if (CanMove)
            {
                if (Library.playerInstance != null)
                {
                    FindPath();

                    if (currentState != walkingState)
                    {
                        if (path != null && path.Count > 0)
                        {
                            SwitchState(walkingState);
                        }
                    }
                }
            }
        }

        #region Find path towards player
        public void FindPath()
        {
            Node startingTile = GetNode(feetHitbox);

            if (startingTile != null && Library.playerInstance.tile != null)
            {
                path = AStar.GetPath(Library.activeRoom.grid, startingTile, Library.playerInstance.tile.node);

                if (path.Count > 0)
                {
                    SwitchState(walkingState);

                    //for (int i = 0; i < path.Count; i++)
                    //{
                    //    path[i].ownerOfNode.color = Color.Cyan;
                    //}
                }
                else
                {
                    if (currentState != idleState)
                    {
                        SwitchState(idleState);
                    }
                }
            }
        }
        #endregion

        public override void OnCollision(ICollidable source)
        {
            base.OnCollision(source);

            #region Collision with player
            if (source is Player p)
            {
                if (!p.invincible)
                {
                    p.ApplyDamage(contactDamage);

                    if (direction != Vector2.Zero)
                    {
                        p.ApplyKnockback(knockbackStrength, direction);
                    }
                    else if (p.direction != Vector2.Zero)
                    {
                        p.ApplyKnockback(knockbackStrength, -p.direction);
                    }
                    else
                    {
                        Vector2 result = Position - p.Position;

                        if (result != Vector2.Zero)
                        {
                            result.Normalize();
                        }
                        else
                        {
                            result = Vector2.One;
                        }

                        p.ApplyKnockback(knockbackStrength, result);
                    }
                }
            }
            #endregion
        }
    }

    #region Idle state
    public class EnemyIdleState : EnemyBaseState
    {
        private Enemy enemy;

        public override void EnterState(Enemy enemyReference)
        {
            enemy = enemyReference;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void ExitState()
        {

        }
    }
    #endregion

    #region Walking state
    public class EnemyWalkingState : EnemyBaseState
    {
        private Enemy enemy;
        private Tile nextTile;
        private int currentTileInPath;

        public override void EnterState(Enemy enemyReference)
        {
            enemy = enemyReference;

            enemy.color = Color.Green;

            currentTileInPath = 0;
            FollowPath();
        }

        public override void Update(GameTime gameTime)
        {
            enemy.Move(gameTime);
            enemy.SetHitboxPosition();

            if (enemy.path.Count > 0)
            {
                if (HasReachedTarget())
                {
                    enemy.SwitchState(enemy.idleState);
                }
                else if (enemy.BoundingBox.Intersects(nextTile.Hitbox))
                {
                    enemy.path.RemoveAt(currentTileInPath);
                    FollowPath();
                }
            }
            else
            {
                enemy.direction = Vector2.Zero;
            }

            if (!enemy.CanMove || enemy.direction == Vector2.Zero)
            {
                enemy.SwitchState(enemy.idleState);
            }
        }

        private void FollowPath()
        {
            nextTile = enemy.path.ElementAt(currentTileInPath).ownerOfNode;
            enemy.direction = GetDirection(nextTile);
        }

        private bool HasReachedTarget()
        {
            if (enemy.BoundingBox.Intersects(enemy.path.Last().ownerOfNode.Hitbox))
            {
                enemy.direction = Vector2.Zero;

                return true;
            }
            else
            {
                return false;
            }
        }

        private Vector2 GetDirection(Tile target)
        {
            Vector2 result = target.Position - enemy.feetHitbox.Location.ToVector2();

            result.Normalize();

            return result;
        }

        public override void ExitState()
        {
            enemy.color = Color.Red;
        }
    }
    #endregion

    #region HurtState
    public class EnemyHurtState : EnemyBaseState
    {
        private Enemy enemy;

        public override void EnterState(Enemy enemyReference)
        {
            enemy = enemyReference;

            if (enemy.Health <= 0)
            {
                enemy.Destroy();
            }

            enemy.invincible = true; 

            Library.StartTimer(enemy.knockBackDuration, RunAfterKnockBack);
        }

        public override void Update(GameTime gameTime)
        {
            enemy.Move(gameTime);

            enemy.SetHitboxPosition();
        }

        private void RunAfterKnockBack()
        {
            enemy.UnlockMovement();
            enemy.invincible = false;

            if (enemy.path != null && enemy.path.Count > 0)
            {
                enemy.SwitchState(enemy.walkingState);
            }
            else
            {
                enemy.direction = Vector2.Zero;
                enemy.SwitchState(enemy.idleState);
            }
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
