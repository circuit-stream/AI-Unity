using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoap
    {
        HashSet<KeyValuePair<string, object>> GetWorldState();

        HashSet<KeyValuePair<string, object>> CreateGoalState();

        void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

        void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<Action> actions);

        void ActionsFinished();

        void PlanAborted(Action aborter);

        bool MoveAgent(Action nextAction);
    }
}