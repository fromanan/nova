﻿using NovaCore.Utilities;
using UnityEngine;

namespace Nova.Utilities
{
    public static class NovaInput
    {
        public static class InputAxis
        {
            public const string MOUSE_X = "Mouse X";
            public const string MOUSE_Y = "Mouse Y";
            public const string HORIZONTAL = "Horizontal";
            public const string VERTICAL = "Vertical";
            public const string MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
        }

        public static Vector3 MousePosition => Input.mousePosition;

        /// <summary>
        /// Returns true if mouse moves, false otherwise
        /// </summary>
        public static bool MouseMoving(float threshold = 0.01f)
        {
            return AxisAboveThreshold(InputAxis.MOUSE_X, threshold) || 
                   AxisAboveThreshold(InputAxis.MOUSE_Y, threshold);
        }

        /// <summary>
        /// Gets the mouses position relative to the origin, as a point in Unity space
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Vector3 GetMouseVector(GameObject origin, Camera camera)
        {
            return MousePosition - camera.WorldToScreenPoint(origin.transform.position);
        }
        
        public static Vector3 GetMouseVector(Transform origin, Camera camera)
        {
            return MousePosition - camera.WorldToScreenPoint(origin.position);
        }

        public static Ray MouseRay(Camera camera)
        {
            return camera.ScreenPointToRay(MousePosition);
        }

        public static bool PlayerMoving(float threshold = 0.01f)
        {
            return AxisAboveThreshold(InputAxis.HORIZONTAL, threshold) || 
                   AxisAboveThreshold(InputAxis.VERTICAL, threshold);
        }

        public static void ToggleCursor(bool cursorOn)
        {
            Cursor.visible = cursorOn;
            Cursor.lockState = cursorOn ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public static bool IsScrolling(float threshold = 0.01f)
        {
            return AxisAboveThreshold(InputAxis.MOUSE_SCROLLWHEEL, threshold);
        }

        public static bool AxisAboveThreshold(string axisName, float threshold = 0.01f)
        {
            return !Math.Between(GetAxisRaw(axisName), threshold);
        }
        
        public static bool RaycastMouse(Camera camera, out RaycastHit hit, LayerMask ignore, float maxDistance = Single.PositiveInfinity)
        {
            return Physics.Raycast(MouseRay(camera), out hit, maxDistance, ~ignore);
        }
        
        public static bool RaycastMouse(Camera camera, out RaycastHit hit, float maxDistance = Single.PositiveInfinity)
        {
            return Physics.Raycast(MouseRay(camera), out hit, maxDistance);
        }
        
        public static Vector3 GetMouseInput(Vector2 sensitivity)
        {
            return new Vector3(-GetAxis(InputAxis.MOUSE_Y) * sensitivity.x, GetAxis(InputAxis.MOUSE_X) * sensitivity.y, 0f);
        }
        
        public static Vector3 GetMovementInput(Vector2 sensitivity)
        {
            return new Vector3(GetAxis(InputAxis.VERTICAL) * sensitivity.x, 0f, GetAxis(InputAxis.HORIZONTAL) * sensitivity.y);
        }

        public static float GetAxisRaw(string axisName) => Input.GetAxisRaw(axisName);
        
        public static float GetAxis(string axisName) => Input.GetAxis(axisName);

        // Mouse Functions
        public static bool LeftMouse        =>      Input.GetMouseButton(0);
        public static bool LeftMouseDown    =>      Input.GetMouseButtonDown(0);
        public static bool LeftMouseUp      =>      Input.GetMouseButtonUp(0);
        
        public static bool RightMouse       =>      Input.GetMouseButton(1);
        public static bool RightMouseDown   =>      Input.GetMouseButtonDown(1);
        public static bool RightMouseUp     =>      Input.GetMouseButtonUp(1);
        
        public static bool MiddleMouse      =>      Input.GetMouseButton(2);
        public static bool MiddleMouseDown  =>      Input.GetMouseButtonDown(2);
        public static bool MiddleMouseUp    =>      Input.GetMouseButtonUp(2);
    }
}
