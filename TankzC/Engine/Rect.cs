using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class Rect
    {
        protected Vector2 relativePosition;

        public Vector2 Position { get { return RigidBody.Position + relativePosition; } }
        public RigidBody RigidBody { get; set; }

        public float HalfWidth { get; protected set; }
        public float HalfHeight { get; protected set; }

        public Rect(Vector2 offset, RigidBody owner, float width, float height)
        {
            relativePosition = offset;
            RigidBody = owner;
            HalfWidth = width / 2;
            HalfHeight = height / 2;
        }

        public bool Collides(Rect rect, ref Collision collisionInfo)
        {
            Vector2 distance = rect.Position - Position;
            
            float deltaX = Math.Abs(distance.X) - (HalfWidth + rect.HalfWidth);
            float deltaY = Math.Abs(distance.Y) - (HalfHeight + rect.HalfHeight);

            if(deltaX <= 0 && deltaY <= 0)
            {
                //setting collision's info
                collisionInfo.Type = Collision.CollisionType.RectsIntersection;
                collisionInfo.Delta = new Vector2(-deltaX, -deltaY);
                return true;
            }
            return false;
        }

        public bool Collides(Circle circle)
        {
            bool collision = false;

            float left = Position.X - HalfWidth;
            float right = Position.X + HalfWidth;
            float top = Position.Y - HalfHeight;
            float bottom = Position.Y + HalfHeight;

            //searching for the nearest point to the circle center
            float nearestX = Math.Max(left, Math.Min(circle.Position.X, right));
            float nearestY = Math.Max(top, Math.Min(circle.Position.Y, bottom));

            //check collision

            float deltaX = circle.Position.X - nearestX;
            float deltaY = circle.Position.Y - nearestY;

            return ((deltaX * deltaX + deltaY * deltaY) <= circle.Ray * circle.Ray);
        }
    }
}
