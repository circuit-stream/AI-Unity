using UnityEngine;
using Action = AI.Goap.Action;

namespace Simulation.Actions
{
    public class DropOffLogsAction: Action
    {
        private bool droppedOffLogs = false;
        private TownCenter townCenterTarget;

	
        public DropOffLogsAction () {
            addPrecondition ("hasLogs", true);
            addEffect ("hasLogs", false); 
            addEffect ("collectLogs", true);
        }
	
	
        public override void reset ()
        {
            droppedOffLogs = false;
        }
	
        public override bool isDone ()
        {
            return droppedOffLogs;
        }
	
        public override bool requiresInRange ()
        {
            return true; 
        }
	
        public override bool checkProceduralPrecondition (GameObject agent)
        {
            TownCenter supplyPiles = FindObjectOfType<TownCenter>();
            target = supplyPiles.gameObject;
            townCenterTarget = supplyPiles;
		
            return townCenterTarget != null;
        }
	
        public override bool perform (GameObject agent)
        {
            Stash stash = agent.GetComponent<Stash>();
            townCenterTarget.numLogs += stash.numLogs;
            droppedOffLogs = true;
            stash.numLogs = 0;
		
            return true;
        }
    }
}
