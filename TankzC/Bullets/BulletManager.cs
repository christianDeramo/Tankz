using System;
using System.Collections.Generic;
using OpenTK;

namespace TankzC
{
    static class BulletManager
    {
        public enum BulletType { StdBullet }

        static Queue<Bullet>[] bullets;

        public static void Init()
        {
            int queueSize = 20;
            bullets = new Queue<Bullet>[(int)BulletType.StdBullet + 1];

            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Queue<Bullet>(queueSize);

                switch ((BulletType)i)
                {
                    case BulletType.StdBullet:
                        for (int j = 0; j < queueSize; j++)
                        {
                            bullets[i].Enqueue(new StdBullet());
                        }
                        break;
                        //case BulletType.BigBlueLaser:
                        //    for (int j = 0; j < queueSize; j++)
                        //    {
                        //        bullets[i].Enqueue(new BigBlueLaser());
                        //    }
                        //    break;
                        //case BulletType.FireGlobe:
                        //    for (int j = 0; j < queueSize; j++)
                        //    {
                        //        bullets[i].Enqueue(new FireGlobeBullet());
                        //    }
                        //    break;
                }
            }
        }

        public static Bullet GetBullet(BulletType type)
        {
            int queueList = (int)type;

            if (bullets[queueList].Count > 0)
            {
                return bullets[queueList].Dequeue();
            }
            return null;
        }

        public static void RestoreBullet(Bullet b)
        {
            bullets[(int)b.Type].Enqueue(b);
        }

        public static void RemoveAll()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Clear();
            }
        }
    }
}
