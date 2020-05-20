using UnityEngine;
using Action = AI.Goap.Action;

namespace Simulation.Actions
{
    public class MineOreAction : Action
    {
        private bool mined = false;
        private IronRock targetRock;

        private float startTime = 0;
        public float miningDuration = 2; 

        public MineOreAction () {
            addPrecondition ("hasTool", true); 
            addPrecondition ("hasOre", false); 
            addEffect ("hasOre", true);
        }
	
	
        public override void reset ()
        {
            mined = false;
            targetRock = null;
            startTime = 0;
        }
	
        public override bool isDone ()
        {
            return mined;
        }
	
        public override bool requiresInRange ()
        {
            return true; 
        }
	
        public override bool checkProceduralPrecondition (GameObject agent)
        {
            IronRock rock = FindObjectOfType<IronRock>();
            target = rock.gameObject;
            return target != null;
        }
	
        public override bool perform (GameObject agent)
        {
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (Time.time - startTime > miningDuration)
            {

                Stash stash = (Stash) agent.GetComponent<Stash>();
                stash.numOre += 2;
                mined = true;
                Tool tool = stash.tool.GetComponent<Tool>();
                tool.use(0.5f);
                if (tool.destroyed()) {
                    Destroy(stash.tool);
                    stash.tool = null;
                }
            }
            return true;
        }
	
    }
}


