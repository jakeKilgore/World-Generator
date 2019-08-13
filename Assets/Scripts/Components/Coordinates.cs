using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct HexCoordinates : IComponentData {
    public int column;
    public int row;
    public int offset;

    private static readonly float WIDTH_MULTIPLE = Mathf.Sqrt(3);
    private static readonly float HEIGHT_MULTIPLE = 2f;
    /// <summary>
    /// Returns the location of the hex in the worldspace.
    /// </summary>
    /// <returns></returns>
    public float2 Position() {
        float height = HEIGHT_MULTIPLE;
        float width = WIDTH_MULTIPLE;
        float posX = width * (column + (row / 2f));
        float posY = height * 3/4 * row;

        return new float2(posX, posY);
    }
}
