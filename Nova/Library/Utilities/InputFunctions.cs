using UnityEngine;

namespace Nova.Library.Utilities
{
    public static class NovaInput
    {
        /// <summary>
        /// Returns true if mouse moves, false otherwise
        /// </summary>
        public static bool CheckForMouseMvt()
        {
            return (int) Input.GetAxisRaw("Mouse X") != 0 || (int) Input.GetAxisRaw("Mouse Y") != 0;
        }

        /// <summary>
        /// Gets the mouses position relative to the origin, as a point in Unity space
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Vector3 GetMouseVector(GameObject origin)
        {
            Vector3 target = Input.mousePosition;

            if (Camera.main == null) return target;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(origin.transform.position);

            return target - objectPos;
        }

        public static bool IsMoving()
        {
            return (int) Input.GetAxisRaw("Horizontal") != 0 || (int) Input.GetAxisRaw("Vertical") != 0;
        }

        public static void ToggleCursor(bool cursorOn)
        {
            Cursor.visible = cursorOn;
            Cursor.lockState = cursorOn ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public static bool IsScrolling()
        {
            return Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.01f;
        }
    }
}
