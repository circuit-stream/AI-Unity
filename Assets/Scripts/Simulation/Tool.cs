using UnityEngine;

namespace Simulation
{
    public class Tool : MonoBehaviour
    {
        public float condition;

        void Start()
        {
            condition = 1;
        }


        public void use(float damage)
        {
            condition -= damage;
        }

        public bool destroyed()
        {
            return condition <= 0f;
        }
    }
}