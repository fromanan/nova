using Nova.Library.Extensions;
using Nova.Library.Utilities;
using UnityEngine;
using Axis = Nova.Library.Utilities.Axes.Axis;

namespace Nova.Library.Attachables
{
    public class StayLevel : MonoBehaviour
    {
        [SerializeField] private Axis axis = Axis.Z;
        [SerializeField] private float adjustmentSpeed = 100f;
        [HideInInspector] public bool disabled;

        private Vector3 axisVector;

        private void Awake()
        {
            axisVector = Axes.VectorFromMask(axis);
        }

        private void FixedUpdate()
        {
            if (disabled) return;
            Vector3 angles = transform.MaskLocalRotation(axisVector);
            if (angles.magnitude > 0.01f)
            {
                transform.FixedSmoothRotateTowards(angles, adjustmentSpeed);
            }
        }
    }
}