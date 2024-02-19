using UnityEngine;

namespace TTCore.Utilities
{
    public class TransformUtils
    {
        public static Vector3 CalculateRelativePosition(Vector3 globalPosition, Vector3 localPosition, Quaternion rotation)
        {
            // 1. Translate: Subtract the local position from the global position to get the offset
            Vector3 offset = globalPosition - localPosition;

            // 2. Rotate Inversely: Apply the inverse rotation to the offset
            Vector3 relativePosition = Quaternion.Inverse(rotation) * offset; 

            return relativePosition;
        }
        
        public static Vector3 CalculateGlobalPosition(Vector3 relativePosition, Vector3 newGlobalPosition, Quaternion newRotation)
        {
            // 1. Rotate: Apply the rotation to the relative position
            Vector3 rotatedPosition = newRotation * relativePosition;

            // 2. Translate: Add the new global position to get the final world position
            Vector3 globalPosition = rotatedPosition + newGlobalPosition;

            return globalPosition;
        }
    }
}