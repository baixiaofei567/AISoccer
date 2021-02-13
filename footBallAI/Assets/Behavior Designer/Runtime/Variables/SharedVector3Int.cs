#if !UNITY_5_3 && !UNITY_5_4 && !UNITY_5_5 && !UNITY_5_6 && !UNITY_2017_1
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedVector3Int : SharedVariable<Vector3Int>
    {
        public static implicit operator SharedVector3Int(Vector3Int value) { return new SharedVector3Int { mValue = value }; }
    }
}
#endif