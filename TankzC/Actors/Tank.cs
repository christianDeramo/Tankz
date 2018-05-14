using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace TankzC
{
    class Tank : Actor
    {
        protected Sprite tracks;
        protected Sprite turret;

        protected Texture tracksTexture;
        protected Texture turretTexture;

        protected Vector2 tracksOffset;
        protected Vector2 turretOffset;

        protected float minAngle;
        protected float maxAngle;

        protected float turretLength;

        protected float moveTime;
        protected Vector2 shakeOffset;

        public Tank(Vector2 spritePosition, string textureName) : base(spritePosition, textureName, DrawManager.Layer.Playground)
        {
            tracksTexture = GfxManager.GetTexture("tracks");
            turretTexture = GfxManager.GetTexture("turret");

            tracks = new Sprite(tracksTexture.Width, tracksTexture.Height);
            turret = new Sprite(turretTexture.Width, turretTexture.Height);

            tracks.pivot = new Vector2(tracks.Width / 2, 0);
            turret.pivot = new Vector2(0, turret.Height / 2);

            tracksOffset = new Vector2(-3, sprite.pivot.Y - 4 - tracks.Height / 2);
            turretOffset = new Vector2(-4, -20);

            minAngle = 0;
            maxAngle = -(float)Math.PI;

            turretLength = turretTexture.Width;

            shakeOffset = new Vector2(0, 0);

            float boxH = sprite.Height + (tracks.Height / 2) - 4;
            float yOff = (boxH - sprite.Height) / 2;
            float circleRadius = (float)Math.Sqrt(sprite.Width * sprite.Width + boxH * boxH) / 2;

            Circle bCircle = new Circle(new Vector2(0, yOff), null, circleRadius);
            Rect bBox = new Rect(new Vector2(0, yOff), null, sprite.Width, boxH);

            RigidBody = new RigidBody(sprite.position, this, bCircle, bBox);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Actor;
            RigidBody.AddCollision((uint)PhysicsManager.ColliderType.Tile);
            RigidBody.AddCollision((uint)PhysicsManager.ColliderType.Bullet);
            RigidBody.AddCollision((uint)PhysicsManager.ColliderType.Actor);
            RigidBody.IsGravityAffected = true;

        }

        public override Bullet Shoot(BulletManager.BulletType type, float speedPercentage = 1.0f)
        {
            Bullet b = BulletManager.GetBullet(type);
            if (b != null)
            {
                float bulletOffsetX = b.Width / 2;

                Vector2 direction = new Vector2((float)Math.Cos(turret.Rotation), (float)Math.Sin(turret.Rotation));

                b.Shoot(turret.position + direction*(turretLength+bulletOffsetX), direction * speedPercentage);
                CameraManager.SetTarget(b);
            }

            return b;
        }

        public override void OnGroundableCollide()
        {
            UpdateSpritesPosition();
        }

        public override void Draw()
        {
            if (IsActive)
            {
                turret.DrawTexture(turretTexture);
                tracks.DrawTexture(tracksTexture);
                sprite.DrawTexture(texture);
            }
        }

        public void UpdateSpritesPosition()
        {
            tracks.position = sprite.position + tracksOffset;
            sprite.position += shakeOffset;
            turret.position = sprite.position + turretOffset;
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();

                if (Velocity.X != 0)
                {
                    moveTime += Game.DeltaTime*20;
                    shakeOffset.Y = (float)Math.Sin(moveTime);
                    shakeOffset.X = (float)Math.Cos(moveTime);
                }
                else
                {
                    shakeOffset = Vector2.Zero;
                }


                UpdateSpritesPosition();
            }

        }
    }
}
