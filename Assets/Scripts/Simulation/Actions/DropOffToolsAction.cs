using UnityEngine;
using Action = AI.Goap.Action;

namespace Simulation.Actions
{
    public class DropOffToolsAction : Action
    {
        private bool droppedOffTools = false;
        private TownCenter targetTownCenter; 

        public DropOffToolsAction()
        {
            addPrecondition("hasNewTools", true);
            addEffect("hasNewTools", false);
            addEffect("collectTools", true);
        }


        public override void reset()
        {
            droppedOffTools = false;
            targetTownCenter = null;
        }

        public override bool isDone()
        {
            return droppedOffTools;
        }

        public override bool requiresInRange()
        {
            return true;
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            TownCenter townCenter = FindObjectOfType<TownCenter>();

            targetTownCenter = townCenter;
            target = targetTownCenter.gameObject;

            return townCenter != null;
        }

        public override bool perform(GameObject agent)
        {
            targetTownCenter.numTools += 2;
            droppedOffTools = true;

            return true;
        }
    }
}