using UnityEngine;
using System.Collections.Generic;

namespace FootBallAI
{
	public class Define
	{
        /// <summary>
        /// 游戏足球桌子的长度
        /// </summary>
        public static int Length = 80;
        /// <summary>
        /// 游戏足球桌子的宽度
        /// </summary>
        public static int Width = 20;
        /// <summary>
        /// 踢球的力量
        /// </summary>
        public static int FORCE = 10;
        /// <summary>
        /// 踢球的大力量
        /// </summary>
        public static int BIG_FORCE = 20;
        /// <summary>
        /// 巡逻的半径
        /// </summary>
        public static float Patrol_Circle = 1.5f;
        /// <summary>
        /// 可以看到球的半径
        /// </summary>
        public static float See_Circle = 10f;
        /// <summary>
        /// 球场左门的位置
        /// </summary>
        public static Vector3 LeftDoorPosition = new Vector3(-Length/2f,0,0);
        /// <summary>
        /// 球场右门的位置
        /// </summary>
        public static Vector3 RightDoorPosition = new Vector3(Length/2f,0,0);
        /// <summary>
        /// 能够踢球的距离
        /// </summary>
        public static float CanKickBallDistance = 1f;


	}
}

