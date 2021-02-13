/// <summary>
/// 防守阵营的踢球策略;
/// </summary>
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FootBallAI
{
    public class KickBallDefence : Action
    {
        /// <summary>
        /// 球员脚本;
        /// </summary>
        private Agent mAgent;
        /// <summary>
        /// 足球脚本;
        /// </summary>
        private Ball Ball;
        /// <summary>
        /// 球的位置;
        /// </summary>
        private Vector3 ballLocation;
        /// <summary>
        /// 球员的位置;
        /// </summary>
        private Vector3 agentLocation;
        private bool bLeft;

        public override void OnStart()
        {
            //获取球员脚本;
            mAgent = GetComponent<Agent>();
            //获取足球脚本;
            Ball = mAgent.GetBall().GetComponent<Ball>();
            bLeft = mAgent.GetTeamDirection();
        }

        public override TaskStatus OnUpdate()
        {
            //获取足球位置;
            ballLocation = mAgent.GetBallLocation();
            //获取球员位置;
            agentLocation = mAgent.transform.position;

            //能进入这个结点一定是看到球了，所以判断球和自己谁离球门更近，如果自己离球门近，就摆阵型，如果球离球门近，就去追球
            //判断能否踢球;
            //cankickdefence就是canseeball
            if (Condition.CanKickDefence(agentLocation, ballLocation))
            {
                //离球很近,可以踢球,就给球一个力;
                if (Condition.CanKickBall(agentLocation, ballLocation))
                {
                    //朝向球;
                    mAgent.transform.LookAt(ballLocation);
                    //根据自己的阵营,给球一个力;
                    bool bLeft = mAgent.GetTeamDirection();
                    if (bLeft)
                    {
                        Ball.AddForceBig(ballLocation, Define.RightDoorPosition);
                    }
                    else
                    {
                        Ball.AddForceBig(ballLocation, Define.LeftDoorPosition);
                    }
                    //返回成功;
                    return TaskStatus.Success;
                }
                else
                {
                    //可以踢球,但是离球较远,就向球移动or摆出阵型;
                    if (Condition.CanDefenceGroup(mAgent.GetTeamDirection(), mAgent.transform.position, ballLocation))
                    {
                        var targetPos = AgentDefenceGroup.Instance.GetDefenceGroupLocation(mAgent, ballLocation, bLeft);
                        //设置自己的目标点;
                        mAgent.SetDestination(targetPos);
                        //返回进行中;
                        return TaskStatus.Running;
                    }
                    else
                    {
                        mAgent.SetDestination(ballLocation);
                    }
                    return TaskStatus.Running;
                }
            }
            //不能踢球返回失败;
            return TaskStatus.Failure;

        }


    }
}     

