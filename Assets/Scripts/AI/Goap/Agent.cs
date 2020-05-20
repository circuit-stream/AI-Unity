using System.Collections.Generic;
using AI.Fsm;
using UnityEngine;
using Utils;

namespace AI.Goap
{
    public sealed class Agent : MonoBehaviour
    {
        private FiniteStateMachine stateMachine;

        private FiniteStateMachine.FSMState idleState;
        private FiniteStateMachine.FSMState moveToState;
        private FiniteStateMachine.FSMState performActionState;

        private HashSet<Action> availableActions;
        private Queue<Action> currentActions;

        private IGoap goapMethods;

        private Planner planner;

        private void Start()
        {
            stateMachine = new FiniteStateMachine();
            availableActions = new HashSet<Action>();
            currentActions = new Queue<Action>();
            planner = new Planner();
            getGoapMethods();
            createIdleState();
            createMoveToState();
            createPerformActionState();
            stateMachine.pushState(idleState);
            loadActions();
        }


        private void Update()
        {
            stateMachine.Update(this.gameObject);
        }


        public void addAction(Action a)
        {
            availableActions.Add(a);
        }

        public void removeAction(Action action)
        {
            availableActions.Remove(action);
        }

        private bool hasActionPlan()
        {
            return currentActions.Count > 0;
        }

        private void createIdleState()
        {
            idleState = (fsm, gameObj) =>
            {
                //GET THE WORLD STATE -> Preconditions are met?
                HashSet<KeyValuePair<string, object>> worldState = goapMethods.GetWorldState();
                //GET THE GOAL STATE 
                HashSet<KeyValuePair<string, object>> goal = goapMethods.CreateGoalState();

                //FIND A PLAN (Action Chain) 
                Queue<Action> plan = planner.plan(gameObject, availableActions, worldState, goal);
                if (plan != null)
                {
                    currentActions = plan;
                    goapMethods.PlanFound(goal, plan);

                    fsm.popState();
                    fsm.pushState(performActionState);
                }
                else
                {
                    Debug.Log("<color=orange>Failed Plan:</color>" + DebugPrinter.prettyPrint(goal));
                    goapMethods.PlanFailed(goal);
                    fsm.popState();
                    fsm.pushState(idleState);
                }
            };
        }

        private void createMoveToState()
        {
            moveToState = (fsm, gameObj) =>
            {
                Action action = currentActions.Peek();
                if (action.requiresInRange() && action.target == null)
                {
                    Debug.Log(
                        "<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                    fsm.popState(); // MOVE
                    fsm.popState(); // PERFORM ACTION
                    fsm.pushState(idleState);
                    return;
                }

                if (goapMethods.MoveAgent(action))
                {
                    fsm.popState();
                }
            };
        }

        private void createPerformActionState()
        {
            performActionState = (fsm, gameObj) =>
            {
                if (!hasActionPlan())
                {
                    Debug.Log("<color=red>Done actions</color>");
                    fsm.popState();
                    fsm.pushState(idleState);
                    goapMethods.ActionsFinished();
                    return;
                }

                Action action = currentActions.Peek();
                if (action.isDone())
                {
                    currentActions.Dequeue();
                }

                if (hasActionPlan())
                {
                    action = currentActions.Peek();
                    bool inRange = action.requiresInRange() ? action.isInRange() : true;
                    if (inRange)
                    {
                        bool success = action.perform(gameObj);

                        if (!success)
                        {
                            fsm.popState();
                            fsm.pushState(idleState);
                            goapMethods.PlanAborted(action);
                        }
                    }
                    else
                    {
                        fsm.pushState(moveToState);
                    }
                }
                else
                {
                    fsm.popState();
                    fsm.pushState(idleState);
                    goapMethods.ActionsFinished();
                }
            };
        }

        private void getGoapMethods()
        {
            foreach (Component comp in gameObject.GetComponents(typeof(Component)))
            {
                if (comp is IGoap)
                {
                    goapMethods = (IGoap) comp;
                    return;
                }
            }
        }

        private void loadActions()
        {
            Action[] actions = gameObject.GetComponents<Action>();
            foreach (Action a in actions)
            {
                availableActions.Add(a);
            }

            Debug.Log("Found actions: " + DebugPrinter.prettyPrint(actions));
        }
    }
}