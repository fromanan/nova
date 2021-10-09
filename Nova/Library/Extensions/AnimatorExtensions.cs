using UnityEngine;

namespace Nova.Library.Extensions
{
    public static class AnimatorExtensions
    {
        public static void SetIK(this Animator animator, AvatarIKGoal goal, Transform target)
        {
            animator.SetIKPosition(goal, target.position);
            animator.SetIKRotation(goal, target.rotation);
        }
        
        public static void SetIK(this Animator animator, AvatarIKGoal goal, Vector3 position, Quaternion rotation)
        {
            animator.SetIKPosition(goal, position);
            animator.SetIKRotation(goal, rotation);
        }
    
        public static void SetIK(this Animator animator, AvatarIKGoal goal, Transform target, float weight)
        {
            animator.SetIKPositionWeight(goal, weight);
            animator.SetIKPosition(goal, target.position);
            animator.SetIKRotationWeight(goal, weight);
            animator.SetIKRotation(goal, target.rotation);
        }
        
        public static void SetIK(this Animator animator, AvatarIKGoal goal, Vector3 position, Quaternion rotation, float weight)
        {
            animator.SetIKPositionWeight(goal, weight);
            animator.SetIKPosition(goal, position);
            animator.SetIKRotationWeight(goal, weight);
            animator.SetIKRotation(goal, rotation);
        }

        public static void SetIKWeights(this Animator animator, AvatarIKGoal goal, float positionWeight,
            float rotationWeight)
        {
            animator.SetIKPositionWeight(goal, positionWeight);
            animator.SetIKRotationWeight(goal, rotationWeight);
        }

        public static float LerpWeights(this Animator animator, float value, bool increasing, float speed, float min = 0f, 
            float max = 1f)
        {
            float targetWeight = increasing ? max : min;
            return Mathf.Lerp(value, targetWeight, speed * Time.deltaTime);
        }
    }
}