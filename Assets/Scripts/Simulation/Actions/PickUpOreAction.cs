using UnityEngine;
using Action = AI.Goap.Action;

namespace Simulation.Actions
{
    public class PickUpOreAction : Action
    {
        private bool hasOre = false;
        private TownCenter targetTownCenter; 
	
        public PickUpOreAction () {
            addPrecondition ("hasOre", false);
            addEffect ("hasOre", true);
        }
	
	
        public override void reset ()
        {
            hasOre = false;
            targetTownCenter = null;
        }
	
        public override bool isDone ()
        {
            return hasOre;
        }
	
        public override bool requiresInRange ()
        {
            return true; 
        }
	
        public override bool checkProceduralPrecondition (GameObject agent)
        {
            TownCenter townCenter = FindObjectOfType<TownCenter>();
            
            targetTownCenter = townCenter;
            target = targetTownCenter.gameObject;
		
            return townCenter != null;
        }
	
        public override bool perform (GameObject agent)
        {
            if (targetTownCenter.numOre >= 3) {
                targetTownCenter.numOre -= 3;
                hasOre = true;

                Stash backpack = (Stash)agent.GetComponent(typeof(Stash));
                backpack.numOre += 3;
			
                return true;
            } else {
                return false;
            }
        }
    }
}
