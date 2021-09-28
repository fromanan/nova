using UnityEngine;

namespace Nova.Library.Attachables
{
    [ExecuteAlways]
    public class Billboard : MonoBehaviour
    {
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (mainCamera)
                transform.LookAt(mainCamera.transform.position, -Vector3.up);
        }
    }
}