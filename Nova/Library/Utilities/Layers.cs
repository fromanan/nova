using System.Collections.Generic;
using UnityEngine;

namespace Nova.Library.Utilities
{
    /**
    * Class that holds layer-related utilities and string constants for layer names.
    * 
    * UPDATE THIS IF YOU UPDATE THE LAYER NAMES.
    */
    public class Layers
    {
        public const string IGNORE_RAYCAST_LAYER_NAME = "Ignore Raycast";
        public const string GROUND_LAYER_NAME = "Ground";
        public const string PLAYER_LAYER_NAME = "Player";
        public const string PLAYER_TOOLS_LAYER_NAME = "PlayerTools";
        public const string WATER_LAYER_NAME = "Water";

        /**
        * Move a gameobject and all of its children onto a given layer
        * 
        * Uses breadth-first search with a queue to move through all child objects
        */
        public static void MoveAllToLayer(GameObject root, int layer)
        {
            // start a queue for BFS
            Queue<Transform> allTransforms = new Queue<Transform>();
            allTransforms.Enqueue(root.transform);

            // go until no more children
            while (allTransforms.Count > 0)
            {
                // take first element out of queue, change the layer, add its children to the queue
                Transform dequeued = allTransforms.Dequeue();
                dequeued.gameObject.layer = layer;
                foreach (Transform child in dequeued.transform)
                {
                    allTransforms.Enqueue(child);
                }
            }
        }

        public static LayerMask PlayerLayerMask =>
            LayerMask.GetMask(PLAYER_LAYER_NAME) | LayerMask.GetMask(PLAYER_TOOLS_LAYER_NAME);

        public static LayerMask GroundLayerMask => LayerMask.GetMask(GROUND_LAYER_NAME);
    }
}