using UnityEngine;

namespace MygaCross
{
    public class TransformPositionPackage : Package
    {
        public int id = 0;
        public Vector3 position = Vector3.zero;

        public TransformPositionPackage(int _id, Vector3 _position) : base("TransformPositionPackage")
        {
            Write(_id);
            WriteVector3(_position);
        }

        public TransformPositionPackage(byte[] _data) : base(_data)
        {
            id = reader.ReadInt();
            position = reader.ReadVector3();
        }
    }
}
