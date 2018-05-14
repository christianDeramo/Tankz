using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class StdBullet : Bullet
    {
        public StdBullet() : base("bullet")
        {
            Type = BulletManager.BulletType.StdBullet;
        }
    }
}
