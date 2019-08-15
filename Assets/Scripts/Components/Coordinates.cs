////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\Components\Coordinates.cs
//
// summary:	Implements the coordinates class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// A component representing a cubic coordinate system for hexagonal tiles. More can be read
/// about this system at: https://www.redblobgames.com/grids/hexagons/.
/// </summary>
///
/// <remarks>   The Vitulus, 8/15/2019. </remarks>
public struct HexCoordinates : IComponentData {

    /// <summary>   The column. </summary>
    public int column;
    /// <summary>   The row. </summary>
    public int row;
    /// <summary>   The offset. </summary>
    public int offset;

    /// <summary>   The width multiple. </summary>
    private static readonly float WIDTH_MULTIPLE = Mathf.Sqrt(3);
    /// <summary>   The height multiple. </summary>
    private static readonly float HEIGHT_MULTIPLE = 2f;

    /// <summary>   Returns the location of the hex in the worldspace. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <returns>   A float2 position in the worldspace. </returns>
    public float2 Position() {
        float height = HEIGHT_MULTIPLE;
        float width = WIDTH_MULTIPLE;
        float posX = width * (column + (row / 2f));
        float posY = height * 3/4 * row;

        return new float2(posX, posY);
    }
}
