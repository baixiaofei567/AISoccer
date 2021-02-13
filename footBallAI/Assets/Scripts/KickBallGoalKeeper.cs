using System.Collections;
/// <summary>
/// 守门员的踢球策略
/// </summary>
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace FootBallAI
{
    public class KickBallGoalKeeper : Action
    {
        ///<summary>
        ///球员的脚本
        ///</summary>
        private Agent mAgent;
        ///<summary>
        ///足球
        ///</summary>
        private Ball Ball;
        ///<summary>
        ///球的位置
        ///</summary>
        private Vector3 ballLoaction;
        ///<summary>
        ///球员位置
        ///</summary>
        private Vector3 agentLoaction;

        public override void OnStart()
        {
            //球员的脚本
            mAgent = GetComponent<Agent>();
            //足球
            Ball = mAgent.GetBall().GetComponent<Ball>();
        }

        public override TaskStatus OnUpdate()
        {
            //获取足球的位置
            ballLoaction = mAgent.GetBallLocation();
            //获取球员的位置
            agentLoaction = mAgent.transform.position;
            //判断足球是否进入了守门员的区域，如果进入了，就返回success
            if (Condition.CanGoalKeeper(ballLoaction))
            {
                if (Condition.CanKickBall(agentLoaction, ballLoaction))
                {
                    //朝向足球
                    mAgent.transform.LookAt(ballLoaction);
                    //根据自己的阵营来给求一个带方向的力
                    bool bLeft = mAgent.GetTeamDirection();
                    if (bLeft)
                    {
                        Ball.AddForceBig(ballLoaction, Define.RightDoorPosition);
                    }
                    else
                    {
                        Ball.AddForceBig(ballLoaction, Define.LeftDoorPosition);
                    }
                    return TaskStatus.Success;
                }
                else
                {
                    //离球较远，向球移动
                    mAgent.SetDestination(ballLoaction);
                    return TaskStatus.Running;
                }
            }
            return TaskStatus.Failure;
        }
    }
}
