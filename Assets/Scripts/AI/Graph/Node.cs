using System.Collections.Generic;
using AI.Goap;

namespace AI.Graph
{
    public class Node
    {
        public Node parent;
        public float runningCost;
        public HashSet<KeyValuePair<string, object>> state;
        public Action action;

        public Node(Node parent, float runningCost, HashSet<KeyValuePair<string, object>> state, Action action)
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.action = action;
        }
    }
}