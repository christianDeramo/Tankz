using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class Player : Tank
    {
        protected float shootDelay;
        protected Bar loadingBar;
        protected Bar nrgBar;
        protected int playerID;
        protected int joystickIndex;

        protected Vector2 barOffset;

        protected float currentLoadingVal;
        protected float maxLoadingVal;
        protected float loadIncrease;

        protected StateMachine stateMachine;

        protected bool IsSpacePressed;
        public int PlayerID { get { return playerID; } protected set { playerID = value; } }
        public bool IsLoading { get; protected set; }
        public Bullet LastShootedBullet { get; protected set; }

        public Player(string fileName, Vector2 spritePosition, int playerIndex=0) : base(spritePosition, fileName)
        {
            playerID = playerIndex;
            joystickIndex = playerIndex;
            
            horizontalSpeed = 300;
            shootDelay = 0.45f;

            loadingBar = new Bar(new Vector2(20, 20), "playerBar", MaxNrg);
            loadingBar.BarOffset = new Vector2(4, 4);
            loadingBar.IsActive = false;

            nrgBar = new Bar(new Vector2(30 + 250 * playerID, 40), "playerBar");
            nrgBar.BarOffset = new Vector2(4, 4);
            nrgBar.SetCamera(CameraManager.GetCamera("GUI"));

            Nrg = 100;

            barOffset = new Vector2(-loadingBar.Width / 2, -150);

            maxLoadingVal = 100;
            loadIncrease = 80;
            
            currentBulletType = BulletManager.BulletType.StdBullet;

            joystickIndex = 0;

            stateMachine = new StateMachine(this);
            stateMachine.RegisterState((int)States.Play, new PlayState());
            stateMachine.RegisterState((int)States.Shoot, new ShootState());
            stateMachine.RegisterState((int)States.Wait, new WaitState());

        }

        public virtual void Play()
        {
            stateMachine.Switch((int)States.Play);
            shootCounter = 0;
        }

        protected override void SetNrg(float newValue)
        {
            base.SetNrg(newValue);
            loadingBar.SetValue(nrg);
        }

        public override Bullet Shoot(BulletManager.BulletType type, float speedPercentage = 1)
        {
            LastShootedBullet = base.Shoot(type, speedPercentage);
            shootCounter = shootDelay;
            stateMachine.Switch((int)States.Shoot);

            return LastShootedBullet;
        }

        public override void OnCollide(Collision collisionInfo)
        {
            base.OnCollide(collisionInfo);

            if (collisionInfo.Collider is Bullet)
            {
                AddDamage(((Bullet)collisionInfo.Collider).Damage);
                ((Bullet)collisionInfo.Collider).OnDie();
            }

            if (collisionInfo.Collider is Actor)
            {
                float deltaX = collisionInfo.Delta.X;
                float deltaY = collisionInfo.Delta.Y;

                if (deltaX < deltaY)
                {
                    //horizontal collision
                    if (Position.X < collisionInfo.Collider.Position.X)
                        deltaX = -deltaX;//from right

                    Position = new Vector2(Position.X + deltaX, Position.Y);
                    OnGroundableCollide();
                }
                else
                {
                    if (!IsGrounded && ((Groundable)collisionInfo.Collider).IsGrounded)
                    {
                        //vertical collision
                        if (Position.Y < collisionInfo.Collider.Position.Y)
                        {
                            deltaY = -deltaY;//from top
                            OnGrounded();
                        }

                        Position = new Vector2(Position.X, Position.Y + deltaY);
                        OnGroundableCollide();
                    }
                }
            }
        }

        public override bool AddDamage(float damage)
        {
            bool isDead = base.AddDamage(damage);

            if (isDead)
            {
                nrgBar.SetValue(0);
            }
            else
                nrgBar.OnHit(nrg);

            return isDead;
        }

        public override void OnDie()
        {
            //base.OnDie();
            ((PlayScene)Game.CurrentScene).OnPlayerDie(this);
        }

        public void StartLoading()
        {
            loadingBar.IsActive = true;
            IsLoading = true;
            currentLoadingVal = 0;
            loadingBar.Position = Position + barOffset;
        }

        public void StopLoading()
        {
            loadingBar.IsActive = false;
            IsLoading = false;
            IsSpacePressed = false;
        }

        public void UpdateFSM()
        {
            stateMachine.Run();
        }

        public override void Update()
        {
            base.Update();

            if (IsLoading)
            {
                currentLoadingVal += Game.window.deltaTime * loadIncrease;

                if (currentLoadingVal > maxLoadingVal)
                {
                    currentLoadingVal = maxLoadingVal;
                    loadIncrease = -loadIncrease;
                }
                else if (currentLoadingVal < 0)
                {
                    currentLoadingVal = 0;
                    loadIncrease = -loadIncrease;
                }

                loadingBar.SetValue(currentLoadingVal);
            }
        }

        public void Input()
        {
            shootCounter -= Game.DeltaTime;

            if (Game.NumJoysticks > 0)
            {
                Vector2 axis = Game.window.JoystickAxisLeft(joystickIndex);

                RigidBody.Velocity = axis * horizontalSpeed;

                if(shootCounter<=0 && Game.window.JoystickA(joystickIndex))
                {
                    Shoot(currentBulletType);
                }
            }
            else
            {

                if (!IsLoading && Game.window.GetKey(KeyCode.Right))
                {
                    RigidBody.SetXVelocity(horizontalSpeed);
                }
                else if (!IsLoading && Game.window.GetKey(KeyCode.Left))
                {
                    RigidBody.SetXVelocity(-horizontalSpeed);
                }
                else
                {
                    RigidBody.SetXVelocity(0);
                }

                if (Game.window.GetKey(KeyCode.Up))
                {
                    turret.Rotation -= Game.DeltaTime;
                    if (turret.Rotation < maxAngle)
                    {
                        turret.Rotation = maxAngle;
                    }
                }
                else if (Game.window.GetKey(KeyCode.Down))
                {
                    turret.Rotation += Game.DeltaTime;
                    if (turret.Rotation > minAngle)
                    {
                        turret.Rotation = minAngle;
                    }
                }

                if (Game.window.GetKey(KeyCode.R))
                {
                    Position = new Vector2(800, 100);
                }

                if (shootCounter <= 0)
                {
                    if (Game.window.GetKey(KeyCode.Space))
                    {
                        if (!IsSpacePressed)
                        {
                            StartLoading();
                            IsSpacePressed = true;
                        }
                        
                    }
                    else if (IsSpacePressed)
                    {
                        StopLoading();
                        Shoot(currentBulletType,currentLoadingVal/maxLoadingVal);
                        IsSpacePressed = false;
                    }
                }
            }
        }

    }
}
