using UnityEngine;

namespace MygaCross
{
    public class TransformRotationPackage : Package
    {
        public int id = 0;
        public Quaternion rotation = Quaternion.identity;

        public TransformRotationPackage(int _id, Quaternion _rotation) : base("TransformRotationPackage")
        {
            Write(_id);
            WriteQuaternion(_rotation);
        }

        public TransformRotationPackage(byte[] _data) : base(_data)
        {
            id = reader.ReadInt();
            rotation = reader.ReadQuaternion();
        }
    }
}
