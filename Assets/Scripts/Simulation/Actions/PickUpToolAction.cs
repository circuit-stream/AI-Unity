using UnityEngine;
using Action = AI.Goap.Action;

namespace Simulation.Actions
{
    public class PickUpToolAction : Action
    {
        private bool hasTool = false;
        private TownCenter targetTownCenter; 

        public PickUpToolAction () {
            addPrecondition ("hasTool", false);
            addEffect ("hasTool", true); 
        }
        
        public override void reset ()
        {
            hasTool = false;
            targetTownCenter = null;
        }
	
        public override bool isDone ()
        {
            return hasTool;
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
            if (targetTownCenter.numTools > 0) {
                targetTownCenter.numTools -= 1;
                hasTool = true;


                Stash stash = agent.GetComponent<Stash>();
                GameObject prefab = Resources.Load<GameObject> (stash.toolType);
                GameObject tool = Instantiate (prefab, transform.position, transform.rotation) as GameObject;
                stash.tool = tool;
                tool.transform.parent = transform; 

                return true;
            } else {
                return false;
            }
        }

    }
}


