using UnityEngine;

namespace Nova.Library.Extensions
{
    public static class TransformExtensions
    {
        // Employs the Physics Update loop to translate frame independently
        public static void SmoothMoveTowards(this Transform transform, Vector3 destination, float moveSpeed)
        {
            transform.position = Vector3.Lerp(destination, transform.position,
                Mathf.Pow(0.9f, Time.deltaTime * moveSpeed));
        }

        // Employs the Physics Update loop to rotate frame independently
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

        // TODO: Does not always result in the precise center
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

        // Destroys all children objects of a transform
        public static void ClearChildren(this Transform transform, bool playMode = true)
        {
            foreach (Transform child in transform)
            {
                if (playMode) Object.Destroy(child.gameObject);
                else Object.DestroyImmediate(child.gameObject);
            }
        }

        // Transfers child transforms to a new parent transform
        public static void BulkReparent(this Transform parent, Transform[] transforms)
        {
            foreach (Transform child in transforms)
            {
                child.parent = parent;
            }
        }

        // Generates a direction quaternion based on two points
        public static Quaternion DirectionFromPoints(Transform origin, Transform target)
        {
            return Quaternion.LookRotation((target.position - origin.position).normalized);
        }
    }
}