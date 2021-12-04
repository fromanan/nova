using UnityEngine;

namespace Nova.Extensions
{
    /// <summary>
    /// Gizmos Utilities
    /// Source: https://github.com/neuneu9/unity-gizmos-utility
    /// Translated to English
    /// </summary>
    public static class GizmosUtility
    {
        private const int CircleVertexCount = 64;

        /// <summary>
        /// Draw a circle (2D)
        /// </summary>
        /// <param name="center">Central location</param>
        /// <param name="radius">Radius</param>
        public static void DrawWireCircle(Vector3 center, float radius)
        {
            DrawWireRegularPolygon(CircleVertexCount, center, Quaternion.identity, radius);
        }

        /// <summary>
        /// Draw a regular polygon (2D)
        /// </summary>
        /// <param name="vertexCount">Number of corners</param>
        /// <param name="center">Central location</param>
        /// <param name="radius">Radius</param>
        public static void DrawWireRegularPolygon(int vertexCount, Vector3 center, float radius)
        {
            DrawWireRegularPolygon(vertexCount, center, Quaternion.identity, radius);
        }

        /// <summary>
        /// Draw a circle (3D)
        /// </summary>
        /// <param name="center">Central location</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="radius">Radius</param>
        public static void DrawWireCircle(Vector3 center, Quaternion rotation, float radius)
        {
            DrawWireRegularPolygon(CircleVertexCount, center, rotation, radius);
        }

        /// <summary>
        /// Draw a regular polygon (3D)
        /// </summary>
        /// <param name="vertexCount">Number of corners </param>
        /// <param name="center">Central location</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="radius">Radius</param>
        public static void DrawWireRegularPolygon(int vertexCount, Vector3 center, Quaternion rotation, float radius)
        {
            if (vertexCount < 3) return;

            Vector3 previousPosition = Vector3.zero;

            // One-step angle to draw a line 
            float step = 2f * Mathf.PI / vertexCount;
            
            // Start angle to draw a line (If it is an even number, shift it by half a step) 
            float offset = Mathf.PI * 0.5f + (vertexCount % 2 == 0 ? step * 0.5f : 0f);

            for (int i = 0; i <= vertexCount; i++)
            {
                float theta = step * i + offset;

                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);

                Vector3 nextPosition = center + rotation * new Vector3(x, y, 0f);

                if (i > 0)
                {
                    Gizmos.DrawLine(previousPosition, nextPosition);
                }

                previousPosition = nextPosition;
            }
        }
    }
}