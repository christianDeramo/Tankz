using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class Bar:GameObject
    {
        protected float value;
        protected float barWidth;
        protected Sprite frame;
        protected Texture frameTexture;

        public float MaxValue { get; set; }

        public override Vector2 Position { get => base.Position; set { SetXPosition(value.X); SetYPosition(value.Y); } }

        public Vector2 BarOffset
        {
            set
            {
                sprite.position = frame.position + value;
            }
        }

        public virtual void SetValue(float newValue)
        {
            value = newValue;
            ResizeBar();
        }

        public Bar(Vector2 position, string textureName= "playerBar", float maxValue=100, int height = 0) : base(position,textureName,DrawManager.Layer.GUI)
        {
            sprite.pivot = Vector2.Zero;
            barWidth = texture.Width;
            value = MaxValue = maxValue;
            //sprite.scale = new Vector2(texture.Width, 1);

            frameTexture = GfxManager.GetTexture("barFrame");
            frame = new Sprite(frameTexture.Width, frameTexture.Height);
            frame.position = position;

        }

        protected virtual void ResizeBar()
        {
            float scale = value / MaxValue;
            barWidth = texture.Width * scale;
            sprite.scale = new Vector2(scale, 1);
            sprite.SetAdditiveTint(1-scale, scale-1, scale-1, 0);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                frame.DrawTexture(frameTexture);
                sprite.DrawTexture(texture, 0, 0, (int)barWidth, (int)sprite.Height);
            }
        }

        public void OnHit(float nrg)
        {
            sprite.scale = new Vector2(nrg/100, 1);
        }

        public override void SetCamera(Camera camera)
        {
            base.SetCamera(camera);
            frame.Camera = camera;
        }

        public void SetXPosition(float newX)
        {
            float xOffSet = newX-frame.position.X;
            frame.position.X = newX;
            sprite.position.X += xOffSet;
        }

        public void SetYPosition(float newY)
        {
            float yOffSet = newY - frame.position.Y;
            frame.position.Y = newY;
            sprite.position.Y += yOffSet;
        }
    }
}
