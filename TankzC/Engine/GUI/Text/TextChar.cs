using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TankzC
{
    class TextChar : GameObject
    {
        protected char character;
        protected Vector2 textureOffset;
        protected Font font;

        public char Character { get { return character; } set { character = value; textureOffset = font.GetOffset(value); } }
        
        public TextChar(Vector2 spritePosition, char character, Font font) : base(spritePosition, font.TextureName, DrawManager.Layer.GUI, font.CharWidth, font.CharHeight)
        {
            this.font = font;
            sprite.pivot = Vector2.Zero;
            sprite.Camera = CameraManager.GetCamera("GUI");
            Character = character;        }
        
        public override void Draw()
        {
            sprite.DrawTexture(texture, (int)textureOffset.X, (int)textureOffset.Y, font.CharWidth, font.CharHeight);
        }

        public void ScaleChar(float blend)
        {
            sprite.scale = new Vector2(blend, blend);
        }
    }
}
