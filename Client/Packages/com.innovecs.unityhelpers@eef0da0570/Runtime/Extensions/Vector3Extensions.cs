using UnityEngine;

namespace Innovecs.UnityHelpers
{
    public static class Vector3Extensions
    {
        public static Matrix4x4 ToTranslateMatrix(this Vector3 vector)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(1f, 0f, 0f, vector.x));
            matrix.SetRow(1, new Vector4(0f, 1f, 0f, vector.y));
            matrix.SetRow(2, new Vector4(0f, 0f, 1f, vector.z));
            matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }
}