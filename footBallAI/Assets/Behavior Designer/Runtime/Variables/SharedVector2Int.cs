#if !UNITY_5_3 && !UNITY_5_4 && !UNITY_5_5 && !UNITY_5_6 && !UNITY_2017_1
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedVector2Int : SharedVariable<Vector2Int>
    {
        public static implicit operator SharedVector2Int(Vector2Int value) { return new SharedVector2Int { mValue = value }; }
    }
}
#endif