using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMath {
    public static int CheckVerticesInHex(int totalLayers) {
        return 3 * totalLayers * (totalLayers + 1) + 1;
    }

    public static int CheckVerticesInLayer(int currentLayer) {
        return 6 * currentLayer;
    }

    public static int CheckTrianglesInHex(int totalLayers) {
        return totalLayers * (totalLayers + 1) * (totalLayers + 2);
    }

    public static int CheckTrianglesInLayer(int currentLayer) {
        return 3 * currentLayer * (currentLayer + 1);
    }
}
