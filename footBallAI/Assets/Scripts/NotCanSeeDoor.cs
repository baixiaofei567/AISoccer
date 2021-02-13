using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace FootBallAI
{
    public class NotCanSeeDoor : Conditional
    {
        private Agent mAgent;
        private bool isL;

        public override void OnStart()
        {
            mAgent = GetComponent<Agent>();
            isL = mAgent.GetTeamDirection();
        }

        public override TaskStatus OnUpdate()
        {
            if (!Condition.CanSeeDoor(mAgent.transform.position, isL))
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
