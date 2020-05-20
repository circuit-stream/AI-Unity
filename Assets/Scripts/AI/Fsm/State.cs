using UnityEngine;

namespace AI.Fsm
{
    public interface State
    {
        void Update(FiniteStateMachine finiteStateMachine, GameObject gameObject);
    }
}