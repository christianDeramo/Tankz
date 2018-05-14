using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class PlayState:State
    {
        public override void Enter()
        {
            ((PlayScene)Game.CurrentScene).ResetTimer();
        }

        public override void Update()
        {
            if (((PlayScene)Game.CurrentScene).PlayerTimer < 1)
            {
                machine.Switch((int)States.Wait);
                return;
            }

            machine.Owner.Input();
        }

        public override void Exit()
        {
            ((PlayScene)Game.CurrentScene).StopTimer();
        }
    }
}
