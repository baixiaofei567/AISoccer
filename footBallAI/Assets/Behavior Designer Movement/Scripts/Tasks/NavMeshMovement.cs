using UnityEngine;
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
using UnityEngine.AI;
#endif

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public abstract class NavMeshMovement : Movement
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed = 10;
        [Tooltip("The angular speed of the agent")]
        public SharedFloat angularSpeed = 120;
        [Tooltip("The agent has arrived when the destination is less than the specified amount")]
        public SharedFloat arriveDistance = 0.2f;
        [Tooltip("Should the NavMeshAgent be stopped when the task ends?")]
        public SharedBool stopOnTaskEnd = true;
        [Tooltip("Should the NavMeshAgent rotation be updated when the task ends?")]
        public SharedBool updateRotation = true;

        // Component references
        protected NavMeshAgent navMeshAgent;

        /// <summary>
        /// Cache the component references.
        /// </summary>
        public override void OnAwake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Allow pathfinding to resume.
        /// </summary>
        public override void OnStart()
        {
            navMeshAgent.speed = speed.Value;
            navMeshAgent.angularSpeed = angularSpeed.Value;
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
            navMeshAgent.Resume();
#else
            navMeshAgent.isStopped = false;
#endif
            if (!updateRotation.Value) {
                UpdateRotation(true);
            }
        }

        /// <summary>
        /// Set a new pathfinding destination.
        /// </summary>
        /// <param name="destination">The destination to set.</param>
        /// <returns>True if the destination is valid.</returns>
        protected override bool SetDestination(Vector3 destination)
        {
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
            navMeshAgent.Resume();
#else
            navMeshAgent.isStopped = false;
#endif
            return navMeshAgent.SetDestination(destination);
        }

        /// <summary>
        /// Specifies if the rotation should be updated.
        /// </summary>
        /// <param name="update">Should the rotation be updated?</param>
        protected override void UpdateRotation(bool update)
        {
            navMeshAgent.updateRotation = update;
        }

        /// <summary>
        /// Does the agent have a pathfinding path?
        /// </summary>
        /// <returns>True if the agent has a pathfinding path.</returns>
        protected override bool HasPath()
        {
            return navMeshAgent.hasPath && navMeshAgent.remainingDistance > arriveDistance.Value;
        }

        /// <summary>
        /// Returns the velocity of the agent.
        /// </summary>
        /// <returns>The velocity of the agent.</returns>
        protected override Vector3 Velocity()
        {
            return navMeshAgent.velocity;
        }

        /// <summary>
        /// Returns true if the position is a valid pathfinding position.
        /// </summary>
        /// <param name="position">The position to sample.</param>
        /// <returns>True if the position is a valid pathfinding position.</returns>
        protected bool SamplePosition(Vector3 position)
        {
            NavMeshHit hit;
            return NavMesh.SamplePosition(position, out hit, float.MaxValue, NavMesh.AllAreas);
        }

        /// <summary>
        /// Has the agent arrived at the destination?
        /// </summary>
        /// <returns>True if the agent has arrived at the destination.</returns>
        protected override bool HasArrived()
        {
            // The path hasn't been computed yet if the path is pending.
            float remainingDistance;
            if (navMeshAgent.pathPending) {
                remainingDistance = float.PositiveInfinity;
            } else {
                remainingDistance = navMeshAgent.remainingDistance;
            }

            return remainingDistance <= arriveDistance.Value;
        }

        /// <summary>
        /// Stop pathfinding.
        /// </summary>
        protected override void Stop()
        {
            UpdateRotation(updateRotation.Value);
            if (navMeshAgent.hasPath) {
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                navMeshAgent.Stop();
#else
                navMeshAgent.isStopped = true;
#endif
            }
        }

        /// <summary>
        /// The task has ended. Stop moving.
        /// </summary>
        public override void OnEnd()
        {
            if (stopOnTaskEnd.Value) {
                Stop();
            } else {
                UpdateRotation(updateRotation.Value);
            }
        }

        /// <summary>
        /// The behavior tree has ended. Stop moving.
        /// </summary>
        public override void OnBehaviorComplete()
        {
            Stop();
        }

        /// <summary>
        /// Reset the values back to their defaults.
        /// </summary>
        public override void OnReset()
        {
            speed = 10;
            angularSpeed = 120;
            arriveDistance = 1;
            stopOnTaskEnd = true;
        }
    }
}