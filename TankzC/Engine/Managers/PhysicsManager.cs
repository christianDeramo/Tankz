using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    static class PhysicsManager
    {
        public enum ColliderType: uint { Actor=1, Tile=2, Bullet=4}
        static List<RigidBody> items;
        static Collision collisionInfo;

        public static void Init()
        {
            items = new List<RigidBody>();
            collisionInfo = new Collision();
        }


        public static void AddItem(RigidBody item)
        {
            items.Add(item);
        }

        public static void RemoveItem(RigidBody item)
        {
            items.Remove(item);
        }
        public static void RemoveAll()
        {
            items.Clear();
        }

        public static void Update()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].GameObject.IsActive)
                    items[i].Update();
            }
        }

        public static void CheckCollisions()
        {
            for (int i = 0; i < items.Count-1; i++)
            {
                if(items[i].GameObject.IsActive && items[i].IsCollisionsAffected)
                {
                    for (int j = i+1; j < items.Count; j++)
                    {
                        if(items[j].GameObject.IsActive && items[j].IsCollisionsAffected)
                        {
                            bool checkFirst = items[i].CheckCollisionWith(items[j]);
                            bool checkSecond = items[j].CheckCollisionWith(items[i]);

                            if ((checkFirst || checkSecond) && items[i].Collides(items[j],ref collisionInfo))
                            {
                                if (checkFirst)
                                {
                                    collisionInfo.Collider = items[j].GameObject;
                                    items[i].GameObject.OnCollide(collisionInfo);
                                }
                                if (checkSecond)
                                {
                                    collisionInfo.Collider = items[i].GameObject;
                                    items[j].GameObject.OnCollide(collisionInfo);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
