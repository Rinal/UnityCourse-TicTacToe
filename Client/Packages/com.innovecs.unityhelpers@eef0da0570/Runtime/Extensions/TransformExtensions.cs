using UnityEngine;

namespace Innovecs.UnityHelpers
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Calculate distance for <param name="target"></param> transform
        /// </summary>
        public static float GetDistanceTo(this Transform transform, Transform target)
        {
            return Vector3.Distance(transform.position, target.position);
        }

        /// <summary>
        /// Calculate distance for <param name="target"></param> position
        /// </summary>
        public static float GetDistanceTo(this Transform transform, Vector3 target)
        {
            return Vector3.Distance(transform.position, target);
        }

        /// <summary>
        /// Calculate direction to <param name="target"></param> transform
        /// </summary>
        public static Vector3 GetDirectionTo(this Transform transform, Transform target)
        {
            return GetDirectionTo(transform, target.position);
        }

        /// <summary>
        /// Calculate direction to <param name="target"></param> transform
        /// </summary>
        public static Vector3 GetDirectionTo(this Transform transform, Vector3 target)
        {
            Vector3 heading = target - transform.position;
            float distance = heading.magnitude;
            return heading/distance;
        }

        /// <summary>
        /// Return true if <param name="target"></param>  is on the <param name="side"></param> from <param name="transform"></param>
        /// </summary>
        public static bool CheckSideTo(this Transform transform, Vector3 side, Transform target)
        {
            return CheckSideTo(transform, side, target.position);
        }

        /// <summary>
        /// Return true if <param name="target"></param>  is on the <param name="side"></param> from <param name="transform"></param>
        /// </summary>
        public static bool CheckSideTo(this Transform transform, Vector3 side, Vector3 target)
        {
            Vector3 newSide = transform.TransformDirection(side);
            Vector3 toPoint = target - transform.position;
            return !(Vector3.Dot(newSide, toPoint) < 0);
        }

        /// <summary>
        /// Set global transform
        /// </summary>
        public static void SetGlobal(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }

        /// <summary>
        /// Set global transform
        /// </summary>
        public static void SetGlobal(this Transform transform, Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(eulerAngles);
            transform.localScale = scale;
        }

        /// <summary>
        /// Set local transform
        /// </summary>
        public static void SetLocal(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform.localPosition = position;
            transform.localRotation = rotation;
            transform.localScale = scale;
        }

        /// <summary>
        /// Set local transform
        /// </summary>
        public static void SetLocal(this Transform transform, Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            transform.localPosition = position;
            transform.localRotation = Quaternion.Euler(eulerAngles);
            transform.localScale = scale;
        }

        /// <summary>
        /// Set default local transform values
        /// </summary>
        public static void ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Set default global transform values
        /// </summary>
        public static void ResetGlobal(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Set transform "x" postion relative to space orientation
        /// </summary>
        public static void Set_X(this Transform transform, float x, Space space = Space.World)
        {
            if (space == Space.World)
                transform.position = new Vector3(x, transform.position.y, transform.position.z);
            else if (space == Space.Self)
                transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }

        /// <summary>
        /// Set transform "y" postion relative to space orientation
        /// </summary>
        public static void Set_Y(this Transform transform, float y, Space space = Space.World)
        {
            if (space == Space.World)
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
            else if (space == Space.Self)
                transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        /// <summary>
        /// Set transform "z" postion relative to space orientation
        /// </summary>
        public static void Set_Z(this Transform transform, float z, Space space = Space.World)
        {
            if (space == Space.World)
                transform.position = new Vector3(transform.position.x, transform.position.y, z);
            else if (space == Space.Self)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }

        /// <summary>
        /// Find child in inserted transforms
        /// </summary>
        public static Transform FindChildRecursive(this Transform parent, string name)
        {
            foreach (Transform child in parent.transform)
                if (string.Equals(child.name, name))
                    return child;
                else
                {
                    var res = FindChildRecursive(child, name);
                    if (res != null)
                        return res;
                }

            return null;
        }

        /// <summary>
        /// Similar to to 3D function LookAt, but rotate objects only arount z axis
        /// </summary>
        public static void LookAt2D(this Transform transform, Vector3 worldPosition)
        {
            var direction = worldPosition - transform.position;
            direction.Normalize();
            var angleZ = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angleZ - 90f);
        }
    }
}
