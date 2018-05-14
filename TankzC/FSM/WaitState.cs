using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class WaitState:State
    {
        public override void Enter()
        {
            machine.Owner.StopLoading();
            ((PlayScene)Game.CurrentScene).NextPlayer();
        }
    }
}
