using UnityEngine;
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
using UnityEngine.AI;
#endif

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public abstract class NavMeshGroupMovement : GroupMovement
    {
        [Tooltip("All of the agents")]
        public SharedGameObject[] agents = null;
        [Tooltip("The speed of the agents")]
        public SharedFloat speed = 10;
        [Tooltip("The angular speed of the agents")]
        public SharedFloat angularSpeed = 120;

        // A cache of the NavMeshAgents
        private NavMeshAgent[] navMeshAgents;
        protected Transform[] transforms;

        public override void OnStart()
        {
            navMeshAgents = new NavMeshAgent[agents.Length];
            transforms = new Transform[agents.Length];
            for (int i = 0; i < agents.Length; ++i) {
                transforms[i] = agents[i].Value.transform;
                navMeshAgents[i] = agents[i].Value.GetComponent<NavMeshAgent>();
                navMeshAgents[i].speed = speed.Value;
                navMeshAgents[i].angularSpeed = angularSpeed.Value;
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                navMeshAgents[i].Resume();
#else
                navMeshAgents[i].isStopped = false;
#endif
            }
        }

        protected override bool SetDestination(int index, Vector3 target)
        {
            if (navMeshAgents[index].destination == target) {
                return true;
            }
            return navMeshAgents[index].SetDestination(target);
        }

        protected override Vector3 Velocity(int index)
        {
            return navMeshAgents[index].velocity;
        }

        public override void OnEnd()
        {
            // Disable the nav mesh
            for (int i = 0; i < navMeshAgents.Length; ++i) {
                if (navMeshAgents[i] != null) {
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                    navMeshAgents[i].Stop();
#else
                    navMeshAgents[i].isStopped = true;
#endif
                }
            }
        }

        // Reset the public variables
        public override void OnReset()
        {
            agents = null;
        }
    }
}