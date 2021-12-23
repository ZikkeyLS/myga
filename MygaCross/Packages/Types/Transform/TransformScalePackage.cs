using UnityEngine;

namespace MygaCross
{
    public class TransformScalePackage : Package
    {
        public int id = 0;
        public Vector3 scale = Vector3.zero;

        public TransformScalePackage(int _id, Vector3 _scale) : base("TransformScalePackage")
        {
            Write(_id);
            WriteVector3(_scale);
        }

        public TransformScalePackage(byte[] _data) : base(_data)
        {
            id = reader.ReadInt();
            scale = reader.ReadVector3();
        }
    }
}
