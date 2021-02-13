using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime.Tasks;

namespace FootBallAI
{
    public class PatrolGoalKeeper : Action
    {
        private Agent agent;
        ///<summary>
        ///巡逻点集合
        ///</summary>
        private List<Vector3> PatrolPositions = new List<Vector3>();
        ///<summary>
        ///巡逻点
        ///</summary>
        private Vector3 PatrolPos;
        ///<summary>
        ///球员位置
        ///</summary>
        private Vector3 agentPosition;
        ///<summary>
        ///足球位置
        ///</summary>
        private Transform ballLoaction;
        ///<summary>
        ///巡逻点的索引值
        ///</summary>
        private int range;

        public override void OnStart()
        {
            agent = GetComponent<Agent>();
            ballLoaction = agent.GetBall().transform;
            //获取Agent自身的位置
            Vector3 InitPos = agent.transform.position;
            //设置巡逻点集合
            PatrolPositions.Add(new Vector3(InitPos.x, InitPos.y, InitPos.z + Define.Patrol_Circle));
            PatrolPositions.Add(new Vector3(InitPos.x, InitPos.y, InitPos.z - Define.Patrol_Circle));

            //选离自己近的位巡逻点
            float distance = Mathf.Infinity;
            //自己和巡逻点之间的距离差
            float localDistance;
            for(int i = 0; i < PatrolPositions.Count; ++i)
            {
                if((localDistance = Vector3.Magnitude(agent.transform.position - PatrolPositions[i])) < distance)
                {
                    distance = localDistance;
                    range = i;
                }
            }
            //设置巡逻点
            PatrolPos = PatrolPositions[range];
            agent.SetDestination(PatrolPos);
        }

        public override TaskStatus OnUpdate()
        {
            //如果球员移动到了巡逻点，就设置下一个巡逻点
            agentPosition = agent.transform.position;
            if(Mathf.Abs(agentPosition.x - PatrolPos.x) < 1 && Mathf.Abs(agentPosition.z - PatrolPos.z) < 1)
            {
                range = (range + 1) % PatrolPositions.Count;
                PatrolPos = PatrolPositions[range];
            }
            //使球员移动到巡逻点
            agent.SetDestination(PatrolPos);

            //使球员朝向球的位置
            agent.transform.LookAt(ballLoaction);
            return TaskStatus.Running;
        }

    }
}
