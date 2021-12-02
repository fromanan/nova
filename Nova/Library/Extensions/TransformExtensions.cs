using System.Collections.Generic;
using System.Linq;
using Nova.Utilities;
using UnityEngine;

namespace Nova.Extensions
{
    public static class TransformExtensions
    {
        // Employs the Physics Update loop to translate frame independently
        
        // SmoothMoveTowards (Update)
        
        public static void SmoothMoveTowards(this Transform transform, Transform destination, float moveSpeed)
        {
            transform.position = Vector3.Lerp(destination.position, transform.position,
                Mathf.Pow(0.9f, Time.deltaTime * moveSpeed));
        }
        
        public static void SmoothMoveTowards(this Transform transform, Vector3 destination, float moveSpeed)
        {
            transform.position = Vector3.Lerp(destination, transform.position,
                Mathf.Pow(0.9f, Time.deltaTime * moveSpeed));
        }

        public static void SmoothMoveTowards(this Transform transform, float x, float y, float z, float moveSpeed)
        {
            transform.position = Vector3.Lerp(new Vector3(x, y, z), transform.position,
                Mathf.Pow(0.9f, Time.deltaTime * moveSpeed));
        }
        
        // FixedSmoothMoveTowards (FixedUpdate)
        
        public static void FixedSmoothMoveTowards(this Transform transform, Transform destination, float moveSpeed)
        {
            transform.position = Vector3.Lerp(destination.position, transform.position,
                Mathf.Pow(0.9f, Time.fixedDeltaTime * moveSpeed));
        }

        public static void FixedSmoothMoveTowards(this Transform transform, Vector3 destination, float moveSpeed)
        {
            transform.position = Vector3.Lerp(destination, transform.position,
                Mathf.Pow(0.9f, Time.fixedDeltaTime * moveSpeed));
        }

        public static void FixedSmoothMoveTowards(this Transform transform, float x, float y, float z, float moveSpeed)
        {
            transform.position = Vector3.Lerp(new Vector3(x, y, z), transform.position,
                Mathf.Pow(0.9f, Time.fixedDeltaTime * moveSpeed));
        }

        // SmoothRotateTowards (Update)
        
        // Employs the Physics Update loop to rotate frame independently
        /*public static void SmoothRotateTowards(this Transform transform, Vector3 newRotation, float turnRate)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(newRotation),
                turnRate * Time.deltaTime);
        }*/
        
        public static void SmoothRotateTowards(this Transform transform, Transform target, float turnRate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation,
                turnRate * Time.deltaTime);
        }
        
        public static void SmoothRotateTowards(this Transform transform, Vector3 newRotation, float turnRate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation),
                turnRate * Time.deltaTime);
        }
        
        public static void SmoothRotateTowards(this Transform transform, float x, float y, float z, float turnRate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(x, y, z),
                turnRate * Time.deltaTime);
        }

        public static void SmoothRotateTowards(this Transform transform, Quaternion newRotation, float turnRate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnRate * Time.deltaTime);
        }
        
        // FixedSmoothMoveTowards (FixedUpdate)
        
        public static void FixedSmoothRotateTowards(this Transform transform, Transform target, float turnRate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation,
                turnRate * Time.fixedDeltaTime);
        }

        public static void FixedSmoothRotateTowards(this Transform transform, Vector3 newRotation, float turnRate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation),
                turnRate * Time.fixedDeltaTime);
        }
        
        public static void FixedSmoothRotateTowards(this Transform transform, float x, float y, float z, float turnRate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(x, y, z),
                turnRate * Time.fixedDeltaTime);
        }

        public static void FixedSmoothRotateTowards(this Transform transform, Quaternion newRotation, float turnRate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnRate * Time.fixedDeltaTime);
        }
        
        
        public static void RotateTowards(this Transform transform, Transform target, float rotateSpeed)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }

        public static bool ComparePosition(this Transform transform, Vector3 target, float tolerance = 0.01f)
        {
            return Vector3.Distance(transform.position, target) < tolerance;
        }
        
        public static IEnumerable<Transform> GetChildren(this Transform transform) => transform.Cast<Transform>();

        public static void ChangeLayersRecursively(this Transform transform, LayerMask layerMask)
        {
            int layer = (int) Mathf.Log(layerMask, 2f);
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

        public static bool ComparePosition(this Transform transform, Transform target, float tolerance = 0.01f)
        {
            return Vector3.Distance(transform.position, target.position) < tolerance;
        }

        public static bool CompareRotation(this Transform transform, Quaternion target, float tolerance = 0.01f)
        {
            return 1f - Mathf.Abs(Quaternion.Dot(transform.rotation, target)) < tolerance;
        }

        public static bool CompareRotation(this Transform transform, Transform target, float tolerance = 0.01f)
        {
            return 1f - Mathf.Abs(Quaternion.Dot(transform.rotation, target.rotation)) < tolerance;
        }

        public static float Distance(this Transform a, Transform b)
        {
            return Vector3.Distance(a.position, b.position);
        }

        public static float Distance(this Transform a, Vector3 b)
        {
            return Vector3.Distance(a.position, b);
        }

        // Returns a vector that points from a to b
        public static Vector3 Direction(this Transform a, Transform b)
        {
            return b.position - a.position;
        }

        public static Vector3 Direction(this Transform a, Vector3 b)
        {
            return b - a.position;
        }

        public static string Format(this Transform transform)
        {
            return $"{transform.name} ({transform.tag})";
        }

        public static void Set(this Transform a, Transform b)
        {
            a.position = b.position;
            a.rotation = b.rotation;
        }

        public static void Set(this Transform transform, Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public static void Set(this Transform transform, Vector3 position)
        {
            transform.position = position;
        }

        public static void SetLocal(this Transform a, Transform b)
        {
            a.localPosition = b.localPosition;
            a.localRotation = b.localRotation;
        }

        public static void SetLocal(this Transform transform, Vector3 position, Quaternion rotation)
        {
            transform.localPosition = position;
            transform.localRotation = rotation;
        }

        public static void SetLocal(this Transform transform, Vector3 position)
        {
            transform.localPosition = position;
        }
        
        public static Vector3 MaskRotation(this Transform transform, Axes.Axis axis)
        {
            return transform.eulerAngles.Hadamard(Axes.VectorFromMask(axis));
        }
        
        public static Vector3 MaskRotation(this Transform transform, Vector3 vector)
        {
            return transform.eulerAngles.Hadamard(vector);
        }
        
        public static Vector3 MaskLocalRotation(this Transform transform, Axes.Axis axis)
        {
            return transform.localEulerAngles.Hadamard(Axes.VectorFromMask(axis));
        }
        
        public static Vector3 MaskLocalRotation(this Transform transform, Vector3 vector)
        {
            return transform.localEulerAngles.Hadamard(vector);
        }
    }
}