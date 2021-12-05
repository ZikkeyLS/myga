using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public static class MathConvert
    {
        public static Vector3 ToVector3(this MVector3 _vector) => new Vector3(_vector.x, _vector.y, _vector.z);
        public static Quaternion ToQuaternion( this MQuaternion _quaternion) => new Quaternion(_quaternion.x, _quaternion.y, _quaternion.z, _quaternion.w);

        public static MVector3 ToMVector3(this Vector3 _vector) => new MVector3(_vector.x, _vector.y, _vector.z);
        public static MQuaternion ToMQuaternion(this Quaternion _quaternion) => new MQuaternion(_quaternion.x, _quaternion.y, _quaternion.z, _quaternion.w);
    }
}
