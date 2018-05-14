using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    abstract class Bullet : GameObject
    {
        protected float shootSpeed;
        protected Vector2 textureOffset;
        protected float damage;

        public BulletManager.BulletType Type { get; protected set; }
        public float Damage { get { return damage; } }

        public Bullet(string textureName="bullets", int spriteWidth=0, int spriteHeight=0) : base(Vector2.Zero, textureName,DrawManager.Layer.Foreground, spriteWidth, spriteHeight)
        {
            shootSpeed = 900;
            IsActive = false;
            RigidBody = new RigidBody(sprite.position, this);
            RigidBody.IsGravityAffected = true;
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Bullet;
            //actors and tiles collision
            RigidBody.SetCollisionMask((uint)PhysicsManager.ColliderType.Actor | (uint)PhysicsManager.ColliderType.Tile);
            damage = 10;
        }

        public override void Draw()
        {
            if(IsActive)
                sprite.DrawTexture(texture, (int)textureOffset.X, (int)textureOffset.Y, Width, Height);
        }

        public virtual void Shoot(Vector2 startPos, Vector2 direction)
        {
            IsActive = true;
            Position = startPos;
            Velocity = direction * shootSpeed;
        }

        public virtual void OnDie()
        {
            IsActive = false;
            //restore bullet
            BulletManager.RestoreBullet(this);
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();

                if (Y- Height/2 > Game.window.Height || X - Width / 2 >= Game.window.Width*2.0f || X + Width / 2 < -Game.window.Width * 2.0f)
                {
                    OnDie();
                }
                else
                {
                    sprite.Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
                }
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Tile)
            {
                collisionInfo.Collider.IsActive = false;
                OnDie();
            }
        }


    }
}
