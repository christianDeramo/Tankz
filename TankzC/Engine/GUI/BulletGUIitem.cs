using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TankzC
{
    class BulletGUIitem : GUIitem
    {
        protected int numBullets;
        protected TextObject numBulletsText;

        public bool IsAvailable { get; set; }
        public bool IsInfinite { get; set; }

        public int NumBullets
        {
            get
            {
                return numBullets;
            }

            set
            {
                int oldVal = numBullets;
                numBullets = value;

                if (numBullets == 0)
                {
                    IsAvailable = false;
                    OnBulletUnavailable();
                }

                else if (oldVal == 0)
                {
                    IsAvailable = true;
                    OnBulletAvailable();
                }
                numBulletsText.Text = numBullets.ToString();
            }
        }

        public BulletGUIitem(Vector2 spritePosition, string textureName, int numBull, bool infinite) : base(spritePosition, textureName, DrawManager.Layer.GUI)
        {
            numBullets = numBull;
            numBulletsText.Position = new Vector2(Position.X - Width / 2, Position.Y + Height / 2);
            IsInfinite = infinite;
            IsAvailable = true;
        }

        public void OnBulletAvailable()
        {
            numBulletsText.IsActive = true;
            sprite.SetAdditiveTint(Vector4.One);
        }

        public void OnBulletUnavailable()
        {
            numBulletsText.IsActive = false;
            sprite.SetAdditiveTint(new Vector4(1, 0, 0, 0.4f));
        }

        public int DecreaseBullets()
        {
            return NumBullets = numBullets - 1;
        }
    }
}
