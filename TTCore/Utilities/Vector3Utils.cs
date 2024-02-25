using UnityEngine;

namespace TTCore.Utilities;

public class Vector3Utils
{
    public static Vector3 Random(float min = 0, float max = 1)
    {
        return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max),
            UnityEngine.Random.Range(min, max));
    }
}