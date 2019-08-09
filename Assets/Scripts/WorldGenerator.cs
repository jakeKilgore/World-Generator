using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NoiseFilter noiseFilter = new NoiseFilter();
        TileEntityFactory.Generate(noiseFilter);
    }
}
