using UnityEngine;

namespace MygaCross
{
    public class TransformPackage : Package
    {
        public int id = 0;
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.zero;

        public TransformPackage(int _id, Vector3 _position, Quaternion _rotation, Vector3 _scale) : base("TransformPackage")
        {
            Write(_id);
            WriteVector3(_position);
            WriteQuaternion(_rotation);
            WriteVector3(_scale);
        }

        public TransformPackage(byte[] _data) : base(_data)
        {
            id = reader.ReadInt();
            position = reader.ReadVector3();
            rotation = reader.ReadQuaternion();
            scale = reader.ReadVector3();
        }
    }
}
