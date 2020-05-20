using UnityEngine;
using Action = AI.Goap.Action;

namespace Simulation.Actions
{
    public class ForgeToolAction : Action
    {
        private bool forged = false;
        
        private float startTime = 0;
        public float forgeDuration = 2; 
	
        public ForgeToolAction () {
            addPrecondition ("hasOre", true);
            addEffect ("hasNewTools", true);
        }

        public override void reset ()
        {
            forged = false;
            startTime = 0;
        }
	
        public override bool isDone ()
        {
            return forged;
        }
	
        public override bool requiresInRange ()
        {
            return true; 
        }
	
        public override bool checkProceduralPrecondition (GameObject agent)
        {
            Forge forge = FindObjectOfType<Forge>();
            target = forge.gameObject;
            return forge != null;
        }
	
        public override bool perform (GameObject agent)
        {
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (Time.time - startTime > forgeDuration)
            {
                Stash stash = agent.GetComponent<Stash>();
                stash.numOre = 0;
                forged = true;
            }
            return true;
        }
	
    }
}
