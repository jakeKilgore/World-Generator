using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public struct NoiseFilter
{
    public float Evaluate(float2 coords) {
        return noise.snoise(coords / 2);
    }
}
