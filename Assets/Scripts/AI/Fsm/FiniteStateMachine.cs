using System.Collections.Generic;
using UnityEngine;

namespace AI.Fsm
{
    public class FiniteStateMachine {

        private Stack<FSMState> stateStack = new Stack<FSMState> ();

        public delegate void FSMState (FiniteStateMachine finiteStateMachine, GameObject gameObject);
	

        public void Update (GameObject gameObject) {
            if (stateStack.Peek() != null)
                stateStack.Peek().Invoke (this, gameObject);
        }

        public void pushState(FSMState state) {
            stateStack.Push (state);
        }

        public void popState() {
            stateStack.Pop ();
        }
    }
}
