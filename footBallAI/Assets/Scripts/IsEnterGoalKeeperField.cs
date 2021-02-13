using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace FootBallAI
{
    public class IsEnterGoalKeeperField : Conditional
    {
        ///<summary>
        ///球员的脚本
        ///</summary>
        private Agent mAgent;
        ///<summary>
        ///足球的脚本
        ///</summary>
        private Ball Ball;
        /// <summary>
        /// 足球的位置
        /// </summary>
        private Vector3 ballLoaction;
        ///<summary>
        ///球员的位置
        ///</summary>
        private Vector3 agentLoaction;

        public override void OnStart()
        {
            //获取球员脚本
            mAgent = GetComponent<Agent>();
            //获取足球脚本
            Ball = mAgent.GetBall().GetComponent<Ball>();
        }

        public override TaskStatus OnUpdate()
        {
            //获取足球位置
            ballLoaction = mAgent.GetBallLocation();
            //获取球员位置
            agentLoaction = mAgent.transform.position;
            //判断球是否进入了守门员的范围
            if (Condition.CanGoalKeeper(ballLoaction))
            {
                mAgent.gameObject.GetComponent<NavMeshAgent>().speed = 20;
                mAgent.gameObject.GetComponent<NavMeshAgent>().acceleration = 18;
                return TaskStatus.Success;
            }
            else
            {
                mAgent.GetComponent<NavMeshAgent>().speed = 10;
                mAgent.gameObject.GetComponent<NavMeshAgent>().acceleration = 8;
                return TaskStatus.Failure;
            }
        }
    }
}
