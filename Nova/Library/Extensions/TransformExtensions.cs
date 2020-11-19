using UnityEngine;

namespace Nova.Library.Extensions
{
    public static class TransformExtensions
    {
        public static void SmoothMoveTowards(this Transform transform, Vector3 destination, float moveSpeed)
        {
            transform.position = Vector3.Lerp(destination, transform.position,
                Mathf.Pow(0.9f, Time.deltaTime * moveSpeed));
        }

        public static void SmoothRotateTowards(this Transform transform, Vector3 newRotation, float turnRate)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(newRotation),
                turnRate * Time.deltaTime);
        }

        public static void RotateTowards(this Transform transform, Transform target, float rotateSpeed)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }

        public static bool ComparePosition(this Transform transform, Vector3 target, float tolerance)
        {
            return Vector3.Distance(transform.position, target) < tolerance;
        }

        public static void ChangeLayersRecursively(this Transform transform, LayerMask layerMask)
        {
            int layer = (int) Mathf.Log(layerMask, 2);
            transform.gameObject.layer = layer;
            foreach (Transform child in transform)
            {
                child.ChangeLayersRecursively(layerMask);
            }
        }

        public static Vector3 Center(this Transform transform)
        {
            Vector3 sum = transform.position;

            foreach (Transform child in transform)
            {
                //Debug.Log(child.Center().ToString("F4"));
                sum += (transform.position - child.Center()) * 0.5f;
            }

            Debug.Log(sum.ToString("F4"));

            return sum;
        }

        public static void ClearChildren(this Transform transform, bool playMode = true)
        {
            foreach (Transform child in transform)
            {
                if (playMode) GameObject.Destroy(child.gameObject);
                else GameObject.DestroyImmediate(child.gameObject);
            }
        }

        public static void BulkReparent(this Transform parent, Transform[] transforms)
        {
            foreach (Transform child in transforms)
            {
                child.parent = parent;
            }
        }

        public static Quaternion DirectionFromPoints(Transform origin, Transform target)
        {
            return Quaternion.LookRotation((target.position - origin.position).normalized);
        }
    }
}