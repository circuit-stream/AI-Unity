using System.Collections.Generic;

namespace Simulation.Agents
{
    public class Logger : RTSCharacter
    {
        public override HashSet<KeyValuePair<string,object>> CreateGoalState () {
            HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
            goal.Add(new KeyValuePair<string, object>("collectLogs", true ));
            return goal;
        }

    }
}

