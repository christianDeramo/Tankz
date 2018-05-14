using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TankzC
{
    abstract class Groundable : GameObject
    {
        public bool IsGrounded
        {
            get { return !RigidBody.IsGravityAffected; }
            set { RigidBody.IsGravityAffected = !value; }
        }
        public Groundable(Vector2 spritePosition, string textureName, DrawManager.Layer drawLayer = DrawManager.Layer.Playground, int spriteWidth = 0, int spriteHeight = 0) : base(spritePosition, textureName, drawLayer, spriteWidth, spriteHeight)
        {
        }

        public virtual void OnGrounded()
        {
            IsGrounded = true;
            RigidBody.SetYVelocity(0);
        }

        public virtual void OnGroundableCollide()
        {

        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Groundable)
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

        public override void Update()
        {
            base.Update();

            if (!IsGrounded)
            {
                float minY = ((PlayScene)Game.CurrentScene).MinY;

                if (Position.Y + RigidBody.HalfHeight > minY)
                {
                    Position = new Vector2(Position.X, minY - RigidBody.HalfHeight);
                    OnGrounded();
                }
            }
            else
            {
                IsGrounded = false;
            }
        }
    }
}
