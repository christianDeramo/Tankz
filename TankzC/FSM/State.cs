using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    enum States
    {
        Play,
        Shoot,
        Wait
    }

    abstract class State
    {

        protected StateMachine machine;

        public virtual void Enter()
        {

        }
        public virtual void Exit()
        {

        }
        public virtual void Update()
        {

        }
        public void AssignStateMachine(StateMachine stateMachine)
        {
            this.machine = stateMachine;
        }
    }
}
