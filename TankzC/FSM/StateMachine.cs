using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class StateMachine
    {
        private Dictionary<int, State> states;
        private State currentState;
        private Player owner;

        public Player Owner
        {
            get
            {
                return owner;
            }
        }

        public StateMachine(Player owner)
        {
            this.states = new Dictionary<int, State>();
            this.owner = owner;
        }
        public void RegisterState(int id, State state)
        {
            states.Add(id, state);
            state.AssignStateMachine(this);
        }
        public void Switch(int id)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            currentState = states[id];
            currentState.Enter();
        }
        public void Run()
        {
            if (currentState != null)
            {
                currentState.Update();
            }
        }
    }
}
