using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace FootBallAI
{
    public class PassBall : Action
    {
        //如果自己是离球门最近的球员，就failure
        //如果不是就传球给最近的球员，并且success

        private Agent mAgent;
        private bool bLeft;
        private Vector3 ballLoaction;//也要判断是否能踢球，才能判断是否能传球，在看不到门的情况下如果能踢球，就传给最近的人
        private Agent nearPlayer;

        private Ball ball;

        public override void OnStart()
        {
            mAgent = GetComponent<Agent>();
            bLeft = mAgent.GetTeamDirection();
            ballLoaction = mAgent.GetBallLocation();
            ball = mAgent.GetBall().GetComponent<Ball>();
        }

        public override TaskStatus OnUpdate()
        {
            nearPlayer = AgentAttackGroup.Instance.findNear(mAgent, bLeft);
            if(nearPlayer.GetNumber() == mAgent.GetNumber())
            {
                return TaskStatus.Failure;
            }
            else
            {
                if (Condition.CanKickBall(mAgent.transform.position, ballLoaction))
                {
                    mAgent.transform.LookAt(ballLoaction);
                    ball.AddForce(ballLoaction,nearPlayer.transform.position);
                    return TaskStatus.Success;
                }
                return TaskStatus.Failure;
            }
        }
    }
}
