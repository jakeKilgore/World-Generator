using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class NoiseFilter
{
    public NoiseFilter() {

    }

    public float Evalutate(float2 coords) {
        return noise.snoise(coords);
    }
}
