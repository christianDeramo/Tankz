using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    interface IDrawable
    {
        DrawManager.Layer Layer { get;}
        void Draw();
    }
}
