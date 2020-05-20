using System.Collections.Generic;

namespace Simulation.Agents
{
    public class Miner : RTSCharacter
    {
        public override HashSet<KeyValuePair<string,object>> CreateGoalState () {
            HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
            goal.Add(new KeyValuePair<string, object>("collectOre", true ));
            return goal;
        }

    }
}

