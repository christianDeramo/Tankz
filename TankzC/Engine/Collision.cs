using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    struct Collision
    {
        public enum CollisionType { None, RectsIntersection}

        public CollisionType Type;
        public Vector2 Delta;
        public GameObject Collider;
    }
}
