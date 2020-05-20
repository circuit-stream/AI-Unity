using UnityEngine;
using Action = AI.Goap.Action;

namespace Simulation.Actions
{
    public class DropOffOreAction : Action
    {
        private bool droppedOffOre = false;
        private TownCenter townCenterTarget; 
	
        public DropOffOreAction () {
            addPrecondition ("hasOre", true);
            addEffect ("hasOre", false);
            addEffect ("collectOre", true); 
        }
        
        public override void reset ()
        {
            droppedOffOre = false;
            townCenterTarget = null;
        }
	
        public override bool isDone ()
        {
            return droppedOffOre;
        }
	
        public override bool requiresInRange ()
        {
            return true; 
        }
	
        public override bool checkProceduralPrecondition (GameObject agent)
        {
            TownCenter townCenter = FindObjectOfType<TownCenter>();
            townCenterTarget = townCenter;
            target = townCenterTarget.gameObject;
		
            return townCenter != null;
        }
	
        public override bool perform (GameObject agent)
        {
            Stash backpack = agent.GetComponent<Stash>();
            townCenterTarget.numOre += backpack.numOre;
            droppedOffOre = true;
            backpack.numOre = 0;

            return true;
        }
    }
}
