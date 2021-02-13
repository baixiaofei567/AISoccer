using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FootBallAI
{
    public class MyPatrol : Action
    {
        /// <summary>
        /// The agent.
        /// </summary>
        private Agent agent;
        /// <summary>
        /// NavMesh组件;
        /// </summary>
		private NavMeshAgent navMeshAgent;
        /// <summary>
        /// 巡逻点集合;
        /// </summary>
        private List<Vector3> PatrolPositions = new List<Vector3>();
        /// <summary>
        /// The patrol position.
        /// </summary>
        private Vector3 PatrolPos;
        /// <summary>
        /// The agent的位置;
        /// </summary>
		private Vector3 agentPosition;
        /// <summary>
        /// The patrol range.
        /// </summary>
        private int range;

        /// <summary>
        /// 定义巡逻的范围的半径
        /// </summary>
        [SerializeField] float Patrol_Circle = Define.Patrol_Circle;

        /// <summary>
        /// 初始化函数;
        /// </summary>
        public override void OnAwake()
        {
            agent = GetComponent<Agent>();

            // 设置巡逻点集合;
            var InitPos = agent.transform.position;
            var PatrolPos1 = new Vector3(InitPos.x + Patrol_Circle, InitPos.y, InitPos.z + Patrol_Circle);
            var PatrolPos2 = new Vector3(InitPos.x + Patrol_Circle, InitPos.y, InitPos.z - Patrol_Circle);
            var PatrolPos3 = new Vector3(InitPos.x - Patrol_Circle, InitPos.y, InitPos.z - Patrol_Circle);
            var PatrolPos4 = new Vector3(InitPos.x - Patrol_Circle, InitPos.y, InitPos.z + Patrol_Circle);
            PatrolPositions.Add(PatrolPos1);
            PatrolPositions.Add(PatrolPos2);
            PatrolPositions.Add(PatrolPos3);
            PatrolPositions.Add(PatrolPos4);
        }

        /// <summary>
        /// 开始函数;
        /// </summary>
        public override void OnStart()
        {
            agent = GetComponent<Agent>();
            navMeshAgent = agent.GetNavAgent();

            // 找出离自己最近的巡逻点;
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < PatrolPositions.Count; ++i)
            {
                if ((localDistance = Vector3.Magnitude(agent.transform.position - PatrolPositions[i])) < distance)
                {
                    distance = localDistance;
                    range = i;
                }
            }
            PatrolPos = PatrolPositions[range];

            //设置设置巡逻;
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(PatrolPos);

        }

        public override TaskStatus OnUpdate()
        {
            agentPosition = agent.transform.position;
            // 当自己走到巡逻点了,就重新随机出下一个巡逻点;
            if (Mathf.Abs(agentPosition.x - PatrolPos.x) < 1 && Mathf.Abs(agentPosition.z - PatrolPos.z) < 1)
            {
                range = (range + 1) % PatrolPositions.Count;
                PatrolPos = PatrolPositions[range];
                //Debug.Log("PatrolPos: " + PatrolPos);
            }
            // 将巡逻点设置为Agent的目标点;
            navMeshAgent.SetDestination(PatrolPos);
            return TaskStatus.Running;
        }

    }

}
