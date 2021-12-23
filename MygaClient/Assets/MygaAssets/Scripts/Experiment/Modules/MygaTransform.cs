using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public class MygaTransform : MPAddon
    {
        private Vector3 syncedPosition = Vector3.zero;
        private Quaternion syncedRotation = Quaternion.identity;
        private Vector3 syncedScale = Vector3.zero;

        private void Start()
        {
            ApplyPosition(transform.position);
            ApplyRotation(transform.rotation);
            ApplyScale(transform.localScale);
        }

        public void TrySetTransform(Vector3 _position, Quaternion _rotation, Vector3 _scale)
        {
            TrySetPosition(_position);
            TrySetRotation(_rotation);
            TrySetScale(_scale);

            Client.Send(new TransformPackage(mygaObject.ID, _position, _rotation, _scale));
        }

        public void TrySetPosition(Vector3 _position)
        {
            Client.Send(new TransformPositionPackage(mygaObject.ID, _position));
        }

        public void TrySetRotation(Quaternion _rotation)
        {
            Client.Send(new TransformRotationPackage(mygaObject.ID, _rotation));
        }

        public void TrySetScale(Vector3 _scale)
        {
            Client.Send(new TransformScalePackage(mygaObject.ID, _scale));
        }

        public void ApplyTransform(Vector3 _position, Quaternion _rotation, Vector3 _scale)
        {
            ApplyPosition(_position);
            ApplyRotation(_rotation);
            ApplyScale(_scale);
        }

        public void ApplyPosition(Vector3 _position)
        {
            syncedPosition = _position;
            transform.position = _position;
        }

        public void ApplyRotation(Quaternion _rotation)
        {
            syncedRotation = _rotation;
            transform.rotation = _rotation;
        }

        public void ApplyScale(Vector3 _scale)
        {
            syncedScale = _scale;
            transform.localScale = _scale;
        }
    }
}