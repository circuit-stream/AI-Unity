using System.Collections.Generic;
using AI.Goap;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Simulation.Agents
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class RTSCharacter : MonoBehaviour, IGoap
    {
        public Stash stash;
        public float moveSpeed = 1;


        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private static readonly int SpeedV = Animator.StringToHash("speedv");


        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (stash == null)
                stash = gameObject.AddComponent<Stash>();
            if (stash.tool == null)
            {
                GameObject prefab = Resources.Load<GameObject>(stash.toolType);
                GameObject tool = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
                stash.tool = tool;
                tool.transform.parent = transform;
            }
        }


        public HashSet<KeyValuePair<string, object>> GetWorldState()
        {
            HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

            worldData.Add(new KeyValuePair<string, object>("hasOre", (stash.numOre > 0)));
            worldData.Add(new KeyValuePair<string, object>("hasLogs", (stash.numLogs > 0)));
            worldData.Add(new KeyValuePair<string, object>("hasTool", (stash.tool != null)));

            return worldData;
        }


        public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();


        public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
        {
            
        }

        public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<Action> actions)
        {
            Debug.Log("<color=green>Plan found</color> " + DebugPrinter.prettyPrint(actions));
        }

        public void ActionsFinished()
        {
            Debug.Log("<color=blue>Actions completed</color>");
        }

        public void PlanAborted(Action aborter)
        {
            Debug.Log("<color=red>Plan Aborted</color> " + DebugPrinter.prettyPrint(aborter));
        }

        public bool MoveAgent(Action nextAction)
        {
            navMeshAgent.SetDestination(nextAction.target.transform.position);

            float dist = Vector3.Distance(transform.position, nextAction.target.transform.position);
            if (dist <= 1.5f)
            {
                nextAction.setInRange(true);
                animator.SetFloat(SpeedV, 0);
                return true;
            }

            animator.SetFloat(SpeedV, navMeshAgent.speed);
            return false;
        }
    }
}