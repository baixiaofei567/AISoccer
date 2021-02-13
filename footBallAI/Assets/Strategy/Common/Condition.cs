using UnityEngine;
using System.Collections.Generic;

namespace FootBallAI
{
	public class Condition
	{
		public static bool CanSeeBall(Vector3 agentLocation,Vector3 ballLocation)
		{
			if (Mathf.Abs(agentLocation.x- ballLocation.x)< Define.See_Circle
                &&Mathf.Abs(agentLocation.z- ballLocation.z)<Define.See_Circle) {
				if (ballLocation.y < 0.5f) {
					return true;
				}
			}
			return false;	
		} 

		public static bool CanSeeDoor(Vector3 agentLocation, bool isLeft)
        {
            if (isLeft)
            {
                if ((Define.RightDoorPosition - agentLocation).sqrMagnitude < 400)
                {
					return true;
                }
				return false;
            }
            else
            {
				if((Define.LeftDoorPosition - agentLocation).sqrMagnitude < 400)
                {
                    return true;
                }
                return false;
            }
        }

		public static bool CanKickBall(Vector3 agentLocation,Vector3 ballLocation)
		{
			if(ballLocation.y>=2)
			{
				return false;
			}
			if (Mathf.Abs (agentLocation.x - ballLocation.x) < Define.CanKickBallDistance 
                && Mathf.Abs (agentLocation.z - ballLocation.z) < Define.CanKickBallDistance)
				return true;
			return false;
		}

        /// <summary>
        /// 防守阵营的能否踢球策略;
        /// </summary>
        /// <param name="agentLocation"></param>
        /// <param name="ballLocation"></param>
        /// <returns></returns>
		public static bool CanKickDefence(Vector3 agentLocation,Vector3 ballLocation)
		{ 
			return CanSeeBall(agentLocation,ballLocation);	
		}

        ///<summary>
        ///防守阵型不能踢球的情况下是摆阵型还是去追球,判断自己和球谁离球门更近
        ///</summary>
        ///<param name="ballLoaction"</param>
        ///<returns></returns>
        public static bool CanDefenceGroup(bool bLeft,Vector3 agnetLocation,Vector3 ballLocation)
        {
            if (bLeft)
            {
                //如果球离球门更近就返回false，证明不能摆阵型了，该去追球了
                return Vector3.Distance(ballLocation, Define.LeftDoorPosition) > Vector3.Distance(agnetLocation, Define.LeftDoorPosition);
            }
            else
            {
                return Vector3.Distance(ballLocation, Define.RightDoorPosition) > Vector3.Distance(agnetLocation, Define.RightDoorPosition);
            }
        }
        

        ///<summary>
        ///守门员能够踢球
        ///<param name="ballLoaction"></param>
        ///</summary>
        public static bool CanGoalKeeper(Vector3 ballLoaction)
        {
            if(Mathf.Abs(ballLoaction.x) > Mathf.Abs((Define.Length / 2) * (3 / 4f))){
                return true;
            }
            return false;
        }
	}
}

