using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TankzC
{
    class Circle
    {
        protected Vector2 relativePosition;

        public Vector2 Position { get { return RigidBody.Position + relativePosition; }}
        public RigidBody RigidBody { get; set; }
        public float Ray { get; protected set; }


        public Circle(Vector2 offset, RigidBody owner, float ray)
        {
            relativePosition = offset;
            RigidBody = owner;
            Ray = ray;
        }

        public bool Contains(Vector2 point)
        {
            Vector2 dist = point - Position;

            return dist.Length <= Ray;
        }

        public bool Collides(Circle circle)
        {
            Vector2 dist = circle.Position - Position;
            return dist.Length <= Ray + circle.Ray;
        }


        
    }
}
