using UnityEngine;
using UnityEngine.AI;

namespace Scenes
{
    public class MoveAgent : MonoBehaviour
    {
        private NavMeshAgent agent;
        public GameObject target;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            agent.SetDestination(target.transform.position);
        }

    }
}
