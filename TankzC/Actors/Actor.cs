using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TankzC
{
    abstract class Actor:Groundable
    {
       
        protected float horizontalSpeed;
        protected float shootCounter;
        protected BulletManager.BulletType currentBulletType;
        protected float nrg;

        public float MaxNrg { get; protected set; }

        public float Nrg { get { return nrg; } set { SetNrg(value); } }

        public Actor(Vector2 spritePosition, string textureName, DrawManager.Layer drawLayer = DrawManager.Layer.Playground, int spriteWidth = 0, int spriteHeight = 0) : base(spritePosition, textureName, drawLayer, spriteWidth, spriteHeight)
        {
            sprite.pivot = new Vector2(Width / 2, Height / 2);
            MaxNrg = 100;
            
        }

        public virtual Bullet Shoot(BulletManager.BulletType type, float speedPercentage = 1.0f)
        {
            return null;
        }

        protected virtual void SetNrg(float newValue)
        {
            nrg = newValue;
            if (nrg > MaxNrg)
            {
                nrg = MaxNrg;
            }
        }

        public virtual void OnDie()
        {
            IsActive = false;
        }



        public virtual bool AddDamage(float damage)
        {
            Nrg -= damage;
            if (Nrg <= 0)
            {
                OnDie();
                return true;
            }
            return false;
        }
    }
}
