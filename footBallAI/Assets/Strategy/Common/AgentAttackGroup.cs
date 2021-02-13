using UnityEngine;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FootBallAI
{ 
	public class AgentAttackGroup : MonoBehaviour
	{ 
		public static AgentAttackGroup Instance;

		[SerializeField] List<Agent> mAgentTeamsLeft = new List<Agent>();
		[SerializeField] List<Agent> mAgentTeamsRight = new List<Agent>();

		void Awake()
		{
			Instance = this;
		}

		public List<Agent> GetAgentTeam(bool bLeft)
		{
			if(bLeft)
			{
				return mAgentTeamsLeft;
			}
			return mAgentTeamsRight;
		}
        /// <summary>
        /// 获取自己在攻击阵营里面的位置;
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="targetPosition"></param>
        /// <param name="bLeft"></param>
        /// <returns></returns>
		public Vector3 GetAttackGroupLocation(Agent agent,Vector3 targetPosition,bool bLeft)
		{
            //根据自己的阵营获取球员列表;
			List<Agent> team = GetAgentTeam (bLeft);
		
            //将球员和目标点的距离进行排序,离目标点最近的排在最前面; 
            team.Sort((a, b) => {
                return Vector3.Distance(a.transform.position, targetPosition).CompareTo(Vector3.Distance(b.transform.position, targetPosition));
            });

            //获取自己在排序后的索引值;
            var index = team.FindIndex (a=>a.GetNumber()==agent.GetNumber());

            // 索引值为0,就是离目标点最近的球员;
            if (index == 0)
            {
                return targetPosition; 
			} 
			else 
			{ 
                // 获取离目标点最近的球员的位置;
				var nearstBallAgentLocation = team[0].transform.position;
                int position = (index + 1) / 2;
               
				if((index%2)==0)
				{
                    //如果自己是偶数就让自己在离目标点最近的球员的侧面,球场的下面;
                    return new Vector3(nearstBallAgentLocation.x,0,nearstBallAgentLocation.z - position * 6); 
				}
                //如果自己是奇数就让自己在离目标点最近的球员的侧面,球场的上面;
                return new Vector3(nearstBallAgentLocation.x,0,nearstBallAgentLocation.z + position * 6);
			}
		} 

		//找到离球门最近的球员进行传球
		public Agent findNear(Agent agent, bool bLeft)
        {
			List < Agent >  team = GetAgentTeam(bLeft);
			if (bLeft)
			{
				team.Sort((a, b) =>
				{
					return Vector3.Distance(a.transform.position, Define.RightDoorPosition).CompareTo(Vector3.Distance(b.transform.position, Define.RightDoorPosition));
				});
			}
            else
            {
				team.Sort((a, b) =>
				{
					return Vector3.Distance(a.transform.position, Define.LeftDoorPosition).CompareTo(Vector3.Distance(b.transform.position, Define.LeftDoorPosition));
				});
            }

			return team[0];
        }
	}
}

