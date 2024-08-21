using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace ARPG
{
    public class Player : Entity, IDamageable
    {
        #region State variables
        public PlayerIdleState idleState = new();
        public PlayerMovingState movingState = new();
        public PlayerHurtState hurtState = new();
        #endregion

        public Node currentNode;
        public event EventHandler OnNodeChange;
        public Vector2 prevPosition;

        public Player(Vector2 startingPosition)
        {
            #region Starting variables
            SetPosition(startingPosition);
            speed = 600;
            Health = maxHealth;

            handOffset = new Vector2(48, 0);
            #endregion

            #region Draw variables
            Texture = TextureManager.EntityTexturesPairs[EntityTextures.Player];
            spriteLayer = TextureManager.SpriteLayers[SpriteLayer.Player];
            #endregion

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public override void CallOnInstantiate()
        {
            prevPosition = Vector2.Zero;

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

        public void NodeChanged()
        {
            OnNodeChange?.Invoke(this, EventArgs.Empty);
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

            if (HasPlayerMoved(player.Position, player.prevPosition, TextureManager.tileSize))
            {
                player.NodeChanged();

                Debug.WriteLine("total time taken: " + AStar.instance.totalTimeTaken.ToString() + " amount of requests: " + AStar.instance.amountOfRequests.ToString());
                AStar.instance.amountOfRequests = 0;
                AStar.instance.totalTimeTaken = TimeSpan.Zero;

                player.prevPosition = player.Position;
            }

            if (!player.CanMove || player.direction == Vector2.Zero)
            {
                player.SwitchState(player.idleState);
            }
        }

        private bool HasPlayerMoved(Vector2 currentPosition, Vector2 previousPosition, float minimumMoveAmount)
        {
            if (currentPosition.X < previousPosition.X - minimumMoveAmount || currentPosition.X > previousPosition.X + minimumMoveAmount)
            {
                return true;
            }
            else if (currentPosition.Y < previousPosition.Y - minimumMoveAmount || currentPosition.Y > previousPosition.Y + minimumMoveAmount)
            {
                return true;
            }

            return false;
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
