using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace TankzC
{
    class WeaponsGUI : GameObject
    {
        public WeaponsGUI(Vector2 spritePosition, string textureName, DrawManager.Layer drawLayer = DrawManager.Layer.Playground, int spriteWidth = 0, int spriteHeight = 0) : base(spritePosition, textureName, drawLayer, spriteWidth, spriteHeight)
        {
        }
    }
}
