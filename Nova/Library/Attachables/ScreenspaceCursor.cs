using Nova.Library.Utilities;
using UnityEngine;

namespace Nova.Library.Attachables
{
    public class ScreenspaceCursor : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] private Transform cursorParent;
        [SerializeField] private GameObject highlightCursor;
        [SerializeField] private GameObject selectCursor;

        [Header("Materials")]
        [SerializeField] private MeshRenderer highlightCursorRender;
        [SerializeField] private Material positiveMaterial;
        [SerializeField] private Material negativeMaterial;

        private Camera mainCamera;
        private Vector3 lastPosition;
        
        private bool overValidObject;
        private bool intersectedObject;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // Check if over anything
            overValidObject = RaycastMouse();

            // Place position
            PlaceCursor();

            cursorParent.gameObject.SetActive(overValidObject);

            SwitchCursorMode(overValidObject && NovaInput.LeftMouse);

            // Color
            ColorCursor();
        }

        public bool RaycastMouse()
        {
            bool flag = NovaInput.RaycastMouse(mainCamera, out RaycastHit hit, 10f);
            lastPosition = hit.point;
            intersectedObject = hit.collider.gameObject.layer != 3;
            return flag;
        }

        public void PlaceCursor()
        {
            cursorParent.position = lastPosition;
        }

        public void ColorCursor()
        {
            highlightCursorRender.sharedMaterial = intersectedObject ? positiveMaterial : negativeMaterial;
        }

        public void SwitchCursorMode(bool clicking)
        {
            highlightCursor.SetActive(!clicking);
            selectCursor.SetActive(clicking);
        }
    }
}