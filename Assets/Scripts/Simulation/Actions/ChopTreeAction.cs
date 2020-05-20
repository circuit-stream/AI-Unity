using UnityEngine;
using Action = AI.Goap.Action;

namespace Simulation.Actions
{
    public class ChopTreeAction : Action
    {
        private bool chopped = false;

        private float startTime = 0;
        public float workDuration = 2;

        public ChopTreeAction()
        {
            addPrecondition("hasTool", true);
            addPrecondition("hasLogs", false);
            addEffect("hasLogs", true);
        }


        public override void reset()
        {
            chopped = false;
            startTime = 0;
        }

        public override bool isDone()
        {
            return chopped;
        }

        public override bool requiresInRange()
        {
            return true;
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            Tree tree = FindObjectOfType<Tree>();
            target = tree.gameObject;
            return tree != null;
        }

        public override bool perform(GameObject agent)
        {
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (Time.time - startTime > workDuration)
            {
                Stash backpack = (Stash) agent.GetComponent(typeof(Stash));
                backpack.numLogs += 1;
                chopped = true;
                Tool tool = backpack.tool.GetComponent<Tool>();
                tool.use(0.4f);
                if (tool.destroyed())
                {
                    Destroy(backpack.tool);
                    backpack.tool = null;
                }
            }

            return true;
        }
    }
}