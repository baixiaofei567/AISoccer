using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace FootBallAI
{
   public class CanSeeBallButNoOut : CanSeeBall
    {
        public override TaskStatus OnUpdate()
        {
            //如果能够看到球,就返回成功.否则返回失败;
            if (Condition.CanSeeBall(Agent.transform.position, Ball.transform.position) && Mathf.Abs(Agent.transform.position.x) < 10)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}
