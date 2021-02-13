using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootBallAI
{
    public class AgentDefenceGroup : MonoBehaviour
    {
        public static AgentDefenceGroup Instance;

        [SerializeField] List<Agent> mAgentTeamsLeft = new List<Agent>();
        [SerializeField] List<Agent> mAgentTeamsRight = new List<Agent>();
        List<Agent> team = new List<Agent>();

        void Awake()
        {
            Instance = this;
        }

        public List<Agent> GetAgentTeam(bool bLeft)
        {
            if (bLeft)
            {
                return mAgentTeamsLeft;
            }
            return mAgentTeamsRight;
        }
        /// <summary>
        /// 获取自己在防守阵营里面的位置;
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="targetPosition"></param>
        /// <param name="bLeft"></param>
        /// <returns></returns>
		public Vector3 GetDefenceGroupLocation(Agent agent, Vector3 targetPosition, bool bLeft)
        {
            //根据自己的阵营获取球员列表;
            List<Agent> team = GetAgentTeam(bLeft);

            //将球员和目标点的距离进行排序,离目标点最近的排在最前面; 
            team.Sort((a, b) => {
                return Vector3.Distance(a.transform.position, targetPosition).CompareTo(Vector3.Distance(b.transform.position, targetPosition));
            });

            //获取自己在排序后的索引值;
            var index = team.FindIndex(a => a.GetNumber() == agent.GetNumber());

            // 索引值为0,自己就是离目标点最近的球员,那目标点就是球
            if (index <= 0)
            {
                return targetPosition;
            }
            else
            {
                //将自己排列在比自己离球更近的球员的附近
                var nearsMeAgentLocation = team[index-1].transform.position;

                if (transform.position.z > nearsMeAgentLocation.z)
                {
                    return new Vector3(nearsMeAgentLocation.x, 0, nearsMeAgentLocation.z + 3);
                }

                else
                {
                    return new Vector3(nearsMeAgentLocation.x, 0, nearsMeAgentLocation.z - 3);
                }

            }
        }
    }
}
