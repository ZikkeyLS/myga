using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MygaCross
{
    public struct MVector3
    {
        public float x;
        public float y;
        public float z;

        public MVector3(float _x, float _y)
        {
            x = _x;
            y = _y;
            z = 0;
        }

        public MVector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static readonly MVector3 zero = new MVector3(0, 0, 0);
        public static readonly MVector3 one = new MVector3(1, 1, 1);

        public static MVector3 operator +(MVector3 vector, MVector3 _vector) => new MVector3(vector.x + _vector.x, vector.y + _vector.y, vector.z + _vector.z);
        public static MVector3 operator -(MVector3 vector, MVector3 _vector) => new MVector3(vector.x - _vector.x, vector.y - _vector.y, vector.z - _vector.z);
        public static MVector3 operator *(MVector3 vector, MVector3 _vector) => new MVector3(vector.x * _vector.x, vector.y * _vector.y, vector.z * _vector.z);
        public static MVector3 operator /(MVector3 vector, MVector3 _vector) => new MVector3(vector.x / _vector.x, vector.y / _vector.y, vector.z / _vector.z);
    }

    public struct MVector2
    {
        public float x;
        public float y;
        public float z;

        public MVector2(float _x, float _y)
        {
            x = _x;
            y = _y;
            z = 0;
        }

        public MVector2(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static readonly MVector3 zero = new MVector3(0, 0, 0);
        public static readonly MVector3 one = new MVector3(1, 1, 1);

        public static MVector2 operator +(MVector2 vector, MVector2 _vector) => new MVector2(vector.x + _vector.x, vector.y + _vector.y);
        public static MVector2 operator -(MVector2 vector, MVector2 _vector) => new MVector2(vector.x - _vector.x, vector.y - _vector.y);
        public static MVector2 operator *(MVector2 vector, MVector2 _vector) => new MVector2(vector.x * _vector.x, vector.y * _vector.y);
        public static MVector2 operator /(MVector2 vector, MVector2 _vector) => new MVector2(vector.x / _vector.x, vector.y / _vector.y);
    }

    public struct MQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public MQuaternion(float _x, float _y)
        {
            x = _x;
            y = _y;
            z = 0;
            w = 0;
        }

        public MQuaternion(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
            w = 0;
        }

        public MQuaternion(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public static readonly  MQuaternion zero = new MQuaternion(0, 0, 0, 1);
        public static readonly MQuaternion one = new MQuaternion(1, 1, 1, 1);
    }
}
