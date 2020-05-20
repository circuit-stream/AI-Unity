using System.Collections.Generic;
using AI.Graph;
using UnityEngine;

namespace AI.Goap
{
    public class Planner
    {
        public Queue<Action> plan(GameObject agent,
            HashSet<Action> availableActions,
            HashSet<KeyValuePair<string, object>> worldState,
            HashSet<KeyValuePair<string, object>> goal)
        {
            foreach (Action a in availableActions)
            {
                a.doReset();
            }

            HashSet<Action> usableActions = new HashSet<Action>();
            foreach (Action a in availableActions)
            {
                if (a.checkProceduralPrecondition(agent))
                    usableActions.Add(a);
            }

            List<Node> leaves = new List<Node>();

            Node start = new Node(null, 0, worldState, null);
            bool success = buildGraph(start, leaves, usableActions, goal);

            if (!success)
            {
                Debug.Log("NO PLAN FOUND");
                return null;
            }

            Node cheapest = null;
            foreach (Node leaf in leaves)
            {
                if (cheapest == null)
                    cheapest = leaf;
                else
                {
                    if (leaf.runningCost < cheapest.runningCost)
                        cheapest = leaf;
                }
            }

            List<Action> result = new List<Action>();
            Node n = cheapest;
            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n.action);
                }

                n = n.parent;
            }


            Queue<Action> queue = new Queue<Action>();
            foreach (Action a in result)
            {
                queue.Enqueue(a);
            }


            return queue;
        }


        private bool buildGraph(Node parent, List<Node> leaves, HashSet<Action> usableActions,
            HashSet<KeyValuePair<string, object>> goal)
        {
            bool foundOne = false;

            foreach (Action action in usableActions)
            {
                if (inState(action.Preconditions, parent.state))
                {
                    HashSet<KeyValuePair<string, object>> currentState = populateState(parent.state, action.Effects);
                    Node node = new Node(parent, parent.runningCost + action.cost, currentState, action);

                    if (inState(goal, currentState))
                    {
                        leaves.Add(node);
                        foundOne = true;
                    }
                    else
                    {
                        HashSet<Action> subset = actionSubset(usableActions, action);
                        bool found = buildGraph(node, leaves, subset, goal);
                        if (found)
                            foundOne = true;
                    }
                }
            }

            return foundOne;
        }

        private HashSet<Action> actionSubset(HashSet<Action> actions, Action removeMe)
        {
            HashSet<Action> subset = new HashSet<Action>();
            foreach (Action a in actions)
            {
                if (!a.Equals(removeMe))
                    subset.Add(a);
            }

            return subset;
        }

        private bool inState(HashSet<KeyValuePair<string, object>> test, HashSet<KeyValuePair<string, object>> state)
        {
            bool allMatch = true;
            foreach (KeyValuePair<string, object> t in test)
            {
                bool match = false;
                foreach (KeyValuePair<string, object> s in state)
                {
                    if (s.Equals(t))
                    {
                        match = true;
                        break;
                    }
                }

                if (!match)
                    allMatch = false;
            }

            return allMatch;
        }

        private HashSet<KeyValuePair<string, object>> populateState(HashSet<KeyValuePair<string, object>> currentState,
            HashSet<KeyValuePair<string, object>> stateChange)
        {
            HashSet<KeyValuePair<string, object>> state = new HashSet<KeyValuePair<string, object>>();
            foreach (KeyValuePair<string, object> s in currentState)
            {
                state.Add(new KeyValuePair<string, object>(s.Key, s.Value));
            }

            foreach (KeyValuePair<string, object> change in stateChange)
            {
                bool exists = false;

                foreach (KeyValuePair<string, object> s in state)
                {
                    if (s.Equals(change))
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists)
                {
                    state.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(change.Key); });
                    KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key, change.Value);
                    state.Add(updated);
                }
                else
                {
                    state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
                }
            }

            return state;
        }
    }
}